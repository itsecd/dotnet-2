function addCustomer() {
    let itemToInsert = {
        name: document.getElementById('name').value,
        phoneNumber: document.getElementById('telephone').value,
        address: document.getElementById('address').value
    };
    let itemToInsertJson = JSON.stringify(itemToInsert);
    let xhttp = new XMLHttpRequest();
    xhttp.open("POST", "rest/customers/");
    xhttp.onload = function (){
        document.location='start-customer.html';
    }
    xhttp.setRequestHeader('Content-Type', 'application/json');
    xhttp.send(itemToInsertJson);
}