import { Component, OnInit, TemplateRef, ViewChild } from '@angular/core';

import { NgbDateStruct } from '@ng-bootstrap/ng-bootstrap';
import { TableColumn } from '@swimlane/ngx-datatable';
import { StepBaseComponent } from '../../../../../../shared/components/step-base/step-base.component';
import { UtilitiesService } from 'src/app/core/services/utilities.services';
import { SecretType, CreateSecretRequest, ICreateSecretRequest } from 'src/app/core/services/identity-api.service';
import { ListViewComponent } from 'src/app/shared/components/list-view/list-view.component';
import { ClientWizardModel } from '../../models/client-wizard-model';

@Component({
    selector: 'app-secrets-step',
    templateUrl: './secrets-step.component.html'
})
export class SecretsStepComponent extends StepBaseComponent<ClientWizardModel> implements OnInit {
    @ViewChild('actionsTemplate', { static: true }) private _actionsTemplate: TemplateRef<HTMLElement>;
    @ViewChild('optionalTemplate', { static: true }) private _optionalTemplate: TemplateRef<HTMLElement>;
    @ViewChild('clientSecretsList', { static: true }) private _clientSecretsList: ListViewComponent;
    private _initialSecrets: CreateSecretRequest[];

    constructor(private _utilities: UtilitiesService) {
        super();
    }

    public columns: TableColumn[] = [];
    public rows: CreateSecretRequest[] = [];
    public clientSecret: CreateSecretRequest = new CreateSecretRequest({
        type: SecretType.SharedSecret
    } as ICreateSecretRequest);

    public ngOnInit(): void {
        this._initialSecrets = this.data.form.get('secrets').value as CreateSecretRequest[];
        this.columns = [
            { prop: 'type', name: 'Type', draggable: false, canAutoResize: true, sortable: false, resizeable: false },
            { prop: 'valueText', name: 'Value', draggable: false, canAutoResize: true, sortable: false, resizeable: false },
            { prop: 'expiration', name: 'Expiration', draggable: false, canAutoResize: true, sortable: false, resizeable: false, cellTemplate: this._clientSecretsList.dateTimeTemplate, cellClass: 'd-flex align-items-center' },
            { prop: 'description', name: 'Description', draggable: false, canAutoResize: true, sortable: false, resizeable: false, cellTemplate: this._optionalTemplate, cellClass: 'd-flex align-items-center' },
            { prop: 'value', name: 'Actions', draggable: false, canAutoResize: true, sortable: false, resizeable: false, cellTemplate: this._actionsTemplate, cellClass: 'd-flex align-items-center' }
        ];
        if (this._initialSecrets.length > 0) {
            this.rows = this._initialSecrets;
        }
    }

    public addClientSecret(): void {
        this.formValidated.emit(true);
        if (!this.clientSecret.type || !this.clientSecret.value) {
            return;
        }
        const expiration = (this.clientSecret as any).expiration as NgbDateStruct;
        this.clientSecret.expiration = expiration ? new Date(expiration.year, expiration.month - 1, expiration.day) : undefined;
        (this.clientSecret as any).valueText = 'Value is hidden';
        this.rows.push(this.clientSecret);
        this.rows = [...this.rows];
        this.data.form.get('secrets').setValue(this.rows);
        this.formValidated.emit(false);
        this.clientSecret = new CreateSecretRequest({
            type: SecretType.SharedSecret
        } as ICreateSecretRequest);
    }

    public removeClientSecret(row: CreateSecretRequest): void {
        const index = this.rows.findIndex(x => x.value === row.value);
        if (index >= -1) {
            this.rows.splice(index, 1);
            this.rows = [...this.rows];
        }
    }

    public generateValue(): void {
        this.clientSecret.value = this._utilities.newGuid();
    }

    public isValid(): boolean {
        return true;
    }
}
