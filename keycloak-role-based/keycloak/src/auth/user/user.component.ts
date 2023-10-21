import { Component, OnDestroy, OnInit } from '@angular/core';
import { Subject, Subscription, takeUntil } from 'rxjs';
import { ApiResponse } from '../dtos/group-dto';
import { AuthApisService } from '../services/auth-apis.service';
import { Columns, Config, DefaultConfig } from 'ngx-easy-table';
import { Router } from '@angular/router';

@Component({
  selector: 'app-user',
  templateUrl: './user.component.html',
  styleUrls: ['./user.component.css']
})
export class UserComponent implements OnInit, OnDestroy {

  constructor(public apiService: AuthApisService,
    private router: Router) { }
  private ngUnsubscribe: Subject<void> = new Subject<void>();
  private subs: Subscription[] = [];

  data: any[] = []  ;

  ngOnDestroy() {
    this.subs.forEach((s) => s.unsubscribe());
    this.ngUnsubscribe.next();
    this.ngUnsubscribe.complete();
  }

  public configuration!: Config;
  public columns!: Columns[];

  ngOnInit() {
    this.getTableHeaderName();
    this.configuration = { ...DefaultConfig };
    this.configuration.tableLayout.hover = !this.configuration.tableLayout.hover;
    this.configuration.tableLayout.striped = !this.configuration.tableLayout.striped;
    this.configuration.tableLayout.style = 'big';
    this.configuration.isLoading = true;
    this.configuration.serverPagination = true;
    this.configuration.threeWaySort = true;
    this.configuration.resizeColumn = true;
    this.configuration.rowReorder = true;
    this.configuration.columnReorder = true;
    this.configuration.fixedColumnWidth = false;
    this.configuration = { ...DefaultConfig };

    this.apiService
    .getUsers()
      .pipe(takeUntil(this.ngUnsubscribe))
      .subscribe({
        next:(response:ApiResponse<any[]>) => {

       if(response.statusCode == 200 && response.data){
        this.data = response.data;

        console.log(response);
       }else{
         console.log(response);
       }
         },
        error:(error:any)=> {
          console.log(error);
        }
   });
  }


  getTableHeaderName() {

    this.columns = [
    { key: 'Id', title:  'Id' },
    { key: 'Username', title:  'Username' },
    { key: 'FirstName', title:  'FirstName' },
    ];

  }

  onEditDialogClicked(valueId: string){
    this.router.navigateByUrl("/auth/"+ valueId);
  }

}
