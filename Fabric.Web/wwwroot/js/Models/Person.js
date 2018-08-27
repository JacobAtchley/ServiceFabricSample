export default class {
    constructor(model){
        this.firstName = model.firstName || '';
        this.lastName = model.lastName || '';
        this.emailAddress = model.emailAddress || '';
        this.id = model.id || null;
        this.dateCreated = model.dateCreated ? moment(model.dateCreated) : null;
    }

    get dateCreatedFormatted(){
        return this.dateCreated ? this.dateCreated.format('LLLL') : '';
    }
}