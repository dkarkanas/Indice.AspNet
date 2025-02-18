import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';

import { BaseListComponent, Icons, IResultSet, ListViewType, MenuOption, ViewAction } from '@indice/ng-components';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';
import { Campaign, CampaignsApiService, CampaignResultSet } from 'src/app/core/services/campaigns-api.services';

@Component({
    selector: 'app-campaigns',
    templateUrl: './campaigns.component.html'
})
export class CampaignsComponent extends BaseListComponent<Campaign> implements OnInit {
    constructor(route: ActivatedRoute, router: Router, private api: CampaignsApiService) {
        super(route, router);
        this.view = ListViewType.Table;
        this.pageSize = 10;
        this.sort = 'createdAt';
        this.sortdir = 'desc';
        this.search = '';
        this.sortOptions = [
          new MenuOption('Ημ/νια Δημιουργίας', 'createdAt'),
          new MenuOption('Τίτλος', 'title'),
          new MenuOption('Ενεργή Από', 'activePeriod.from')
        ];
    }

    public newItemLink: string | null = 'create';
    public full = true;

    public ngOnInit(): void {
        super.ngOnInit();
    }

    public loadItems(): Observable<IResultSet<Campaign> | null | undefined> {
        return this.api
            .getCampaigns(undefined, undefined, this.page, this.pageSize, this.sortdir === 'asc' ? this.sort! : this.sort + '-', this.search || undefined)
            .pipe(map((result: CampaignResultSet) => (result as IResultSet<Campaign>)));
    }

    public actionHandler(action: ViewAction): void {
        if (action.icon === Icons.Refresh) {
            this.search = '';
            this.refresh();
        }
    }
}
