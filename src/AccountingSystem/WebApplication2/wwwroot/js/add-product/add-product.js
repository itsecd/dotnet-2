function addCustomer() {
    let itemToInsert = {
        name: document.getElementById('name').value,
        phoneNumber: document.getElementById('price').value,
        address: document.getElementById('date').value
    };
    let itemToInsertJson = JSON.stringify(itemToInsert);
    let xhttp = new XMLHttpRequest();
    xhttp.open("POST", "api/product/");
    xhttp.onload = function (){
        document.location='start-product.html';
    }
    xhttp.setRequestHeader('Content-Type', 'application/json');
    xhttp.send(itemToInsertJson);
}