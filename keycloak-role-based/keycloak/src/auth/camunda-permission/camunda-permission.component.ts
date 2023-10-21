import { Component, OnDestroy, OnInit, ViewChild } from '@angular/core';
import { AuthApisService } from '../services/auth-apis.service';
import { Subject, Subscription, takeUntil } from 'rxjs';
import { ApiResponse, GroupDto, UserGroupsDto, UserDto } from '../dtos/group-dto';
import { TreeItem, TreeviewComponent, TreeviewConfig, TreeviewItem } from '@treeview/ngx-treeview';
import { ActivatedRoute, Router } from '@angular/router';


@Component({
  selector: 'app-camunda-permission',
  templateUrl: './camunda-permission.component.html',
  styleUrls: ['./camunda-permission.component.css']
})
export class CamundaPermissionComponent implements OnInit , OnDestroy {

  constructor(public apiService: AuthApisService,
    private router: Router,
    private route: ActivatedRoute) { }
  private ngUnsubscribe: Subject<void> = new Subject<void>();
  private subs: Subscription[] = [];

  @ViewChild(TreeviewComponent, { static: false }) treeviewComponent!: TreeviewComponent;

  showSubmitButton = false;

  ngOnInit() {

    this.getData();


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
    .getGroups()
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



}
