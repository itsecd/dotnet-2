function addCustomer() {
    let itemToInsert = {
        name: document.getElementById('name').value,
        phone: document.getElementById('telephone').value,
        address: document.getElementById('address').value
    };
    let itemToInsertJson = JSON.stringify(itemToInsert);
    let xhttp = new XMLHttpRequest();
    xhttp.open("POST", "../api/Customer/");
    xhttp.setRequestHeader('Content-Type', 'application/json');
    xhttp.send(itemToInsertJson);
}