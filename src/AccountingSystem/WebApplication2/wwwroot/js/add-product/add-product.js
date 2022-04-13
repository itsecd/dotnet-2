function addProduct() {
    let itemToInsert = {
        name: document.getElementById('name').value,
        phoneNumber: document.getElementById('price').value,
        address: document.getElementById('date').value
    };
    let itemToInsertJson = JSON.stringify(itemToInsert);
    let xhttp = new XMLHttpRequest();
    xhttp.open("POST", "api/Product/");
    xhttp.setRequestHeader('Content-Type', 'application/json');
    xhttp.send(itemToInsertJson);
}