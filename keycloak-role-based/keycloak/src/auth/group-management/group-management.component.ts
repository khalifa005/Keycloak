import { Component, OnDestroy, OnInit, ViewChild } from '@angular/core';
import { AuthApisService } from '../services/auth-apis.service';
import { Subject, Subscription, takeUntil } from 'rxjs';
import { ApiResponse, GroupDto, UserGroupsDto, UserDto } from '../dtos/group-dto';
import { TreeItem, TreeviewComponent, TreeviewConfig, TreeviewItem } from '@treeview/ngx-treeview';
import { ActivatedRoute, Router } from '@angular/router';

@Component({
  selector: 'app-group-management',
  templateUrl: './group-management.component.html',
  styleUrls: ['./group-management.component.css']
})
export class GroupManagementComponent implements OnInit , OnDestroy {

  constructor(public apiService: AuthApisService,
    private router: Router,
    private route: ActivatedRoute) { }
  private ngUnsubscribe: Subject<void> = new Subject<void>();
  private subs: Subscription[] = [];

  @ViewChild(TreeviewComponent, { static: false }) treeviewComponent!: TreeviewComponent;

  selectedUserId!: string;
  showSubmitButton = false;

  ngOnInit() {
    this.selectedUserId = this.route.snapshot.params['id'];

    this.getData();

    this.apiService
    .getUsers()
      .subscribe({
        next:(response:ApiResponse<UserDto[]>) => {

       if(response.statusCode == 200 && response.data){
        this.users = response.data;
        this.selectedUser = this.users.find( x => x.id === this.selectedUserId );

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

  ngOnDestroy() {
    this.subs.forEach((s) => s.unsubscribe());
    this.ngUnsubscribe.next();
    this.ngUnsubscribe.complete();
  }

  data: GroupDto[] = []  ;
  users: UserDto[] = []  ;
  selectedUser: any;

  private getData(): void {

    this.apiService
    .getRepresentationUserGroupsById(this.selectedUserId)
      .pipe(takeUntil(this.ngUnsubscribe))
      .subscribe({
        next:(response:ApiResponse<GroupDto[]>) => {

       if(response.statusCode == 200 && response.data){
        this.data = response.data;
        this.items = this.formateTreeResponse(this.data);
        this.showSubmitButton = this.items.length > 0;
        console.log(this.items);
       }else{
         console.log(response);
       }
         },
        error:(error:any)=> {
          console.log(error);
        }
   });

  }


  items: TreeviewItem[] = [];
  config = TreeviewConfig.create({
      hasCheckBoxes: true,
      hasAllCheckBox: true,
      hasFilter: true,
      hasCollapseExpand: true,
      decoupleChildFromParent: false,
      maxHeight: 400,
      compact: false
    });

    private formateTreeResponse(groupDto : GroupDto[]) : TreeviewItem [] {
      const treeviewItems : TreeviewItem[] =[];
        for (let index = 0; index < groupDto.length; index++) {
          const element = groupDto[index];
          const treeItems = this.buildChildTree(element.subGroups);
          // const parentName = this.translate.currentLang === "ar-SA" ? element.NameAr : element.NameEn;
          const parentName = element.name;
          const extraCategory = new TreeviewItem({ text: parentName , value: element.id, checked: element.checked, children:treeItems }, true);
          extraCategory.correctChecked();
          treeviewItems.push(extraCategory);
        }
        return treeviewItems;
      }

     private buildChildTree(childrens:GroupDto[]) {
      const treeItems : TreeItem[] =[];
        for (let childIndex = 0; childIndex < childrens.length; childIndex++) {
          const childElement = childrens[childIndex];
          // const elementName = this.translate.currentLang === "ar-SA" ? childElement.NameAr : childElement.NameEn;
          const elementName =  childElement.name;
          treeItems.push({text :  elementName , value: childElement.id, checked: childElement.checked , children : this.buildChildTree(childElement.subGroups) });
        }

        return treeItems;
      }

      onSelectedTreeChange(event: any){
        console.log(this.items);

      }


      collectSelectedTreeIds(elementItems : TreeviewItem [] , checkedGroupsIds:string []){
        elementItems.forEach(element => {
          if(element.checked || element.indeterminate){
            checkedGroupsIds.push(element.value);
          }

          const children = element.children;
           if(children!= undefined)
              this.collectSelectedTreeIds(children,checkedGroupsIds);
        });
      }


      saveUserGroupsIds(){

        //-- Validate
        if(this.treeviewComponent.selection.checkedItems.length <= 0){
          // this.toastNotificationService.showToast(NotitficationsDefaultValues.Danger, title, 'Select at least one role function');
          return;
       }


       if(!this.selectedUser){
        //  this.toastNotificationService.showToast(NotitficationsDefaultValues.Danger, title, 'Select User');
         return;
       }

       //--Collect Data
       const userGroups : UserGroupsDto = {};
        userGroups.userId = this.selectedUserId;

        const checkedGroupIds: string [] = [];
        this.treeviewComponent.items.every(x => x.correctChecked());

        this.collectSelectedTreeIds(this.treeviewComponent.items ,checkedGroupIds);

        console.log("collectSelectedTreeIds");
        console.log(checkedGroupIds);
        userGroups.groupIds = checkedGroupIds;

        this.apiService
    .saveRoleFunctions(userGroups)
      .pipe(takeUntil(this.ngUnsubscribe))
      .subscribe({
        next:(response:ApiResponse<any[]>) => {

       if(response.statusCode == 200){
        this.router.navigateByUrl("/users");

       }else{
       }
         },
        error:(error:any)=> {
          console.log(error);
        }
   });

      }

}
