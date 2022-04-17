window.onload = function() {
    getCustomers();
}

function getCustomers() {
    var xhttp = new XMLHttpRequest();
    xhttp.open("GET", "../api/Customer/");
    xhttp.onload = function() {
        var customers = JSON.parse(xhttp.responseText);
        let rows = '';
        for (let i = 0; i < customers.length; i++) {
            rows +=
                "<option value = " + customers[i].customerId + ">" +
                customers[i].name + "</option>";
        }
        document.getElementById("customerChoose").innerHTML = rows;
    }
    xhttp.send();
}

function addOrder() {
    var xhttp = new XMLHttpRequest();
    xhttp.open("GET", "../api/Customer");
    xhttp.onload = function() {
        var customers = JSON.parse(xhttp.responseText);
        for (let i = 0; i < customers.length; i++) {
            if(customers[i].customerId == document.getElementById('customerChoose').value){
                let customerToInsert = {
                    customerID: customers[i].customerId,
                    name: customers[i].name,
                    phoneNumber: customers[i].phone,
                    address: customers[i].address
                };
                let itemToInsert = {
                    customer: customerToInsert,
                    date: document.getElementById('date').value,
                    status: document.getElementById('status').value,
                    products : []
                };
                let itemToInsertJson = JSON.stringify(itemToInsert);
                let xhttpPost = new XMLHttpRequest();
                xhttpPost.open("POST", "../api/Order/");
                xhttpPost.onload = function () {
                    document.location = 'start-order.html';
                }
                xhttpPost.setRequestHeader('Content-Type', 'application/json');
                xhttpPost.send(itemToInsertJson);
            }
        }
    }
    xhttp.send();
}

function addOrderAndProduct() {
    var xhttp = new XMLHttpRequest();
    xhttp.open("GET", "../api/Customer");
    xhttp.onload = function () {
        var customers = JSON.parse(xhttp.responseText);
        for (let i = 0; i < customers.length; i++) {
            if (customers[i].customerId == document.getElementById('customerChoose').value) {
                let customerToInsert = {
                    customerID: customers[i].customerId,
                    name: customers[i].name,
                    phoneNumber: customers[i].phone,
                    address: customers[i].address
                };
                let itemToInsert = {
                    customer: customerToInsert,
                    date: document.getElementById('date').value,
                    status: document.getElementById('status').value,
                    products: []
                };
                let itemToInsertJson = JSON.stringify(itemToInsert);
                let xhttpPost = new XMLHttpRequest();
                xhttpPost.open("POST", "../api/Order/");
                xhttpPost.onload = function () {
                    document.location = 'add-product.html';
                }
                xhttpPost.setRequestHeader('Content-Type', 'application/json');
                xhttpPost.send(itemToInsertJson);
            }
        }
    }
    xhttp.send();
}