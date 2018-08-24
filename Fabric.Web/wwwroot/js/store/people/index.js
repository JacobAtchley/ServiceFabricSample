import Person from '/js/Models/Person.js';
import storeFactory from '/js/store/entityStoreFactory.js';

export default storeFactory({
    mapper: x => new Person(x),
    rootUrl: '/api/graphs/people'
  });