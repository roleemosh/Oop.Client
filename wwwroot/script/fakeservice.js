class Person {
    constructor(employeeID, firstName, lastName, accessLevel) {
        this.EmployeeID = employeeID;
        this.FirstName = firstName;
        this.LastName = lastName;
        this.AccessLevel = accessLevel;
    }
}

export async function createRandomPeople(count) {

    let people = []

    for (let i = 0; i < count; i++) {
        let rndPerson = createRandomPerson();

        people.push(rndPerson);
    }

    const { getAssemblyExports } = await globalThis.getDotnetRuntime(0);
    var exports = await getAssemblyExports('Oop.Client.dll');

    const peopleJson = JSON.stringify(people);
    exports.Oop.Client.Services.FakeService.PopulateFakePeople(peopleJson);
}

export function createRandomPerson() {
    return new Person(
        faker.random.uuid(),
        faker.name.firstName(),
        faker.name.lastName(),
        faker.random.number({
            'min': 0,
            'max': 255
        })
    );
}
