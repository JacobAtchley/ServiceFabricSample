import Person from '/js/Models/Person.js';
import storeFactory from '/js/Store/EntityStoreFactory.js';

export default storeFactory({
    mapper: x => new Person(x),
    rootUrl: '/api/grains/people'
  });