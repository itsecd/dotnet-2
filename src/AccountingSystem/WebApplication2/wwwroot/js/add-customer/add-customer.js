function addCustomer() {
    let itemToInsert = {
        name: document.getElementById('name').value,
        phone: document.getElementById('telephone').value,
        address: document.getElementById('address').value
    };
    let itemToInsertJson = JSON.stringify(itemToInsert);
    let xhttp = new XMLHttpRequest();
    xhttp.open("POST", "https://localhost:5002/api/Customer/");
    xhttp.setRequestHeader('Content-Type', 'application/json');
    xhttp.onload = function () {
        if (xhttp.status == 200) {
            document.location = 'start-customer.html';
        } else {
            if (confirm("Customer don't add. Close?")) {
                document.location = 'start-customer.html';
            }
        }
    }
    xhttp.send(itemToInsertJson);
}