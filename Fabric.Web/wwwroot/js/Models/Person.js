export default class {
    constructor(model){
        this.firstName = model.firstName || '';
        this.lastName = model.lastName || '';
        this.emailAddress = model.emailAddress || '';
    }
}