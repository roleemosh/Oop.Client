export function initialize(collectionName, keyPath) {
    let miltonIndexedDb = indexedDB.open(DATABASE_NAME, CURRENT_VERSION);
    miltonIndexedDb.onupgradeneeded = function () {
        let db = miltonIndexedDb.result;
        db.createObjectStore(collectionName, { keyPath: keyPath });
    }
}


export function populate(collectionName, values) {

    // Check if it's a zero-byte array
    if (values == null || values === undefined) {
        return;
    }

    let miltonIndexedDb = indexedDB.open(DATABASE_NAME, CURRENT_VERSION);

    miltonIndexedDb.onsuccess = function () {
        let transaction = miltonIndexedDb.result.transaction(collectionName, "readwrite");
        let collection = transaction.objectStore(collectionName)

        values.forEach(value => collection.put(value));
    }
}

export function set(collectionName, value) {
    let miltonIndexedDb = indexedDB.open(DATABASE_NAME, CURRENT_VERSION);

    miltonIndexedDb.onsuccess = function () {
        let transaction = miltonIndexedDb.result.transaction(collectionName, "readwrite");
        let collection = transaction.objectStore(collectionName)
        collection.put(value);
    }
}

export async function get(collectionName, id) {
    let request = new Promise((resolve) => {
        let miltonIndexedDb = indexedDB.open(DATABASE_NAME, CURRENT_VERSION);
        miltonIndexedDb.onsuccess = function () {
            let transaction = miltonIndexedDb.result.transaction(collectionName, "readonly");
            let collection = transaction.objectStore(collectionName);
            let result = collection.get(id);

            result.onsuccess = function (e) {
                resolve(result.result);
            }
        }
    });

    let result = await request;

    return result;
}

export async function getAll(collectionName) {

    let request = new Promise((resolve) => {

        let miltonIndexedDb = indexedDB.open(DATABASE_NAME, CURRENT_VERSION);

        miltonIndexedDb.onsuccess = function () {
            let transaction = miltonIndexedDb.result.transaction(collectionName, "readonly");
            let collection = transaction.objectStore(collectionName);
            let result = collection.getAll();

            result.onsuccess = function (e) {
                resolve(result.result);
            }
        }
    });

    let result = await request;

    return result;
}


export async function count(collectionName) {

    let request = new Promise((resolve) => {

        let miltonIndexedDb = indexedDB.open(DATABASE_NAME, CURRENT_VERSION);

        miltonIndexedDb.onsuccess = function () {
            let transaction = miltonIndexedDb.result.transaction(collectionName, "readonly");
            let collection = transaction.objectStore(collectionName);
            // Get a count of the objects in the store
            const countRequest = collection.count();

            countRequest.onsuccess = function (e) {
                resolve(countRequest.result);
            }
        }
    });

    let result = await request;

    return result;
}

let CURRENT_VERSION = 1;
let DATABASE_NAME = "MiltonDB";