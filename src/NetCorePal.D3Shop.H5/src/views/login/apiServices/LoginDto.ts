export class LoginInputDto {
    public constructor(uName: string, pwd: string) {
        this.userName = uName;
        this.password = pwd;
    }

    public userName: string;
    public password: string;
}

export class LoginResultDto {

}

