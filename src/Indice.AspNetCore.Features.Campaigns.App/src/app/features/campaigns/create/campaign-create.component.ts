import { AfterViewInit, ChangeDetectorRef, Component, ElementRef, Inject, OnInit, ViewChild } from '@angular/core';
import { NgForm } from '@angular/forms';
import { Router } from '@angular/router';

import { MenuOption, Modal, ModalService, SideViewLayoutComponent, ToasterService, ToastType } from '@indice/ng-components';
import { map } from 'rxjs/operators';
import * as app from 'src/app/core/models/settings';
import { Campaign, DeliveryChannel, CampaignsApiService, CampaignTypeResultSet, CreateCampaignRequest, Period, ValidationProblemDetails } from 'src/app/core/services/campaigns-api.services';
import { UtilitiesService } from 'src/app/shared/utilities.service';
import { CampaignTypesModalComponent } from '../campaign-types-modal/campaign-types.component';

@Component({
    selector: 'app-campaigns',
    templateUrl: './campaign-create.component.html'
})
export class CampaignCreateComponent implements OnInit, AfterViewInit {
    @ViewChild('campaignForm', { static: false }) private campaignForm!: NgForm;
    @ViewChild('sideViewLayout', { static: false }) private sideViewLayout!: SideViewLayoutComponent;
    @ViewChild('submitBtn', { static: false }) public submitButton!: ElementRef;

    constructor(
        private api: CampaignsApiService,
        private modal: ModalService,
        private router: Router,
        public _utilities: UtilitiesService,
        private changeDetector: ChangeDetectorRef,
        @Inject(ToasterService) private toaster: ToasterService
    ) { }

    public now: Date = new Date();
    public model = new CreateCampaignRequest({
        activePeriod: new Period({ from: this.now }),
        published: true,
        isGlobal: true,
        deliveryChannel: [DeliveryChannel.Inbox],
        title: '',
        content: ''
    });
    public campaignTypes: MenuOption[] = [];
    public campaignTypesModalRef: Modal | undefined;
    public isDevelopment = !app.settings.production;
    public CampaignDeliveryChannel = DeliveryChannel;
    public customDataValid = true;
    public showCustomDataValidation = false;
    public submitInProgress = false;
    public targetOptions: MenuOption[] = [
        new MenuOption('Όλους τους χρήστες', true),
        new MenuOption('Ομάδα χρηστών', false)
    ];

    public ngOnInit(): void {
        this.loadCampaignTypes();
    }

    public ngAfterViewInit(): void {
        this.changeDetector.detectChanges();
    }

    public canSubmit(): boolean {
        return this.campaignForm?.valid === true && this.hasDeliveryChannel();
    }

    public onSubmit(): void {
        if (!this.canSubmit()) {
            return;
        }
        this.submitInProgress = true;
        this.api
            .createCampaign(this.model)
            .subscribe((campaign: Campaign) => {
                this.submitInProgress = false;
                // This is to force reload campaigns page when a new campaign is successfully saved. 
                this.router.navigateByUrl('/', { skipLocationChange: true }).then(() => this.router.navigate(['campaigns']));
                this.toaster.show(ToastType.Success, 'Επιτυχής αποθήκευση', `Η καμπάνια με τίτλο '${campaign.title}' δημιουργήθηκε με επιτυχία.`);
            }, (problemDetails: ValidationProblemDetails) => {
                // This is to force reload campaigns page when a new campaign is successfully saved.
                this.toaster.show(ToastType.Error, 'Αποτυχής αποθήκευση', `${this._utilities.getValidationProblemDetails(problemDetails)}`, 6000);
            });
    }

    public openCampaignTypesModal(): void {
        this.campaignTypesModalRef = this.modal.show(CampaignTypesModalComponent, {
            backdrop: 'static',
            keyboard: false,
            animated: true,
            initialState: {
                campaignTypes: this.campaignTypes.filter(x => x.value != null)
            }
        });
        this.campaignTypesModalRef.onHidden?.subscribe((response: any) => {
            if (response.result.campaignTypesChanged) {
                this.loadCampaignTypes();
            }
        });
    }

    public toDate(event: any): Date | undefined {
        var value = event.target.value
        if (value) {
            return new Date(value);
        }
        return undefined;
    }

    public setIsGlobal(isGlobal: boolean): void {
        this.model.isGlobal = isGlobal;
        if (isGlobal) {
            delete this.model.selectedUserCodes;
        }
    }

    public toUserCodesArray(userCodes: string | undefined): string[] {
        return userCodes ? [...new Set(userCodes.split('\n').filter(x => x !== ''))] : [];
    }

    public toUserCodesString(userCodes: string[] | undefined): string {
        return userCodes ? userCodes.join('\n') : '';
    }

    public toggleDeliveryChannel(deliveryType: DeliveryChannel): void {
        if (deliveryType !== DeliveryChannel.Inbox) {
            this.model.deliveryChannel = this.model.deliveryChannel!.filter(x => x === DeliveryChannel.Inbox || x === deliveryType);
        }
        const index = this.model.deliveryChannel!.findIndex(channel => channel === deliveryType);
        if (index > -1) {
            this.model.deliveryChannel!.splice(index, 1);
        } else {
            this.model.deliveryChannel!.push(deliveryType);
        }
    }

    public hasDeliveryChannel(): boolean {
        return this.model.deliveryChannel!.length > 0;
    }

    public containsDeliveryChannel(deliveryType: DeliveryChannel): boolean {
        return this.model.deliveryChannel!.indexOf(deliveryType) > -1;
    }

    public setCampaignCustomData(metadataJson: string): void {
        if (!metadataJson || metadataJson === '') {
            if ('data' in this.model) {
                delete this.model.data;
            }
            return;
        }
        try {
            const data = JSON.parse(metadataJson);
            this.customDataValid = true;
            this.model.data = data;
        } catch (error) {
            this.customDataValid = false;
        }
    }

    public onCustomDataFocusOut(): void {
        this.showCustomDataValidation = true;
    }

    private loadCampaignTypes(): void {
        this.campaignTypes = [];
        this.api
            .getCampaignTypes()
            .pipe(map((campaignTypes: CampaignTypeResultSet) => {
                if (campaignTypes.items) {
                    this.campaignTypes = campaignTypes.items.map(type => new MenuOption(type.name || '', type.id));
                    this.campaignTypes.unshift(new MenuOption('Παρακαλώ επιλέξτε...', null));
                }
            }))
            .subscribe();
    }
}
