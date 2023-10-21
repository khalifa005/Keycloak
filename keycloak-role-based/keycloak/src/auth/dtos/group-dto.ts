export class GroupDto {
  id: number;
  name: string;
  path: string;
  attributes?: any;
  realmRoles?: any;
  subGroups: GroupDto[];

  disabled? : boolean;
  checked?: boolean;

  constructor(id: number, nameEn : string, nameAr : string, disabled : boolean, checked : boolean , childrens : GroupDto[], path: string ) {
    this.id = id;
    this.name = nameEn;
    this.subGroups = childrens;
    this.disabled = disabled;
    this.checked = checked;
    this.path = path;
  }
}


export class UserDto {
  id: string;
  username: string;
  firstName: string;

  constructor(id: string, username : string, firstName : string) {
    this.id = id;
    this.username = username;
    this.firstName = firstName;
  }
}


export class UserGroupsDto {
  userId?:string;
  groupIds?: string[];
}



export class ApiResponse<t> {
  data?: t;
  errors?: string[] | null;
  statusCode?: number;
  errorMessageAr?: string | null;
  errorMessage?: string | null;
}

