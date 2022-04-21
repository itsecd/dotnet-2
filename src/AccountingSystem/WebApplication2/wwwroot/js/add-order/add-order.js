window.onload = function() {
    getCustomers();
}

function getCustomers() {
    var xhttp = new XMLHttpRequest();
    xhttp.open("GET", "https://localhost:5002/api/Customer/");
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
    if (CheckValidationData()) {
        var xhttp = new XMLHttpRequest();
        xhttp.open("GET", "https://localhost:5002/api/Customer");
        xhttp.onload = function () {
            var customers = JSON.parse(xhttp.responseText);
            for (let i = 0; i < customers.length; i++) {
                if (customers[i].customerId == document.getElementById('customerChoose').value) {
                    let customerToInsert = {
                        customerID: customers[i].customerId,
                        name: customers[i].name,
                        phone: customers[i].phone,
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
                    xhttpPost.open("POST", "https://localhost:5002/api/Order/");
                    xhttpPost.onload = function () {
                        if (xhttpPost.status == 200) {
                            document.location = 'start-order.html';
                        } else {
                            if (confirm("Order don't add. Close?")) {
                                document.location = 'start-order.html';
                            }
                        }
                    }
                    xhttpPost.setRequestHeader('Content-Type', 'application/json');
                    xhttpPost.send(itemToInsertJson);
                }
            }
        }
        xhttp.send();
    }
}

function addOrderAndProduct() {
    if (CheckValidationData()) {
        var xhttp = new XMLHttpRequest();
        xhttp.open("GET", "https://localhost:5002/api/Customer");
        xhttp.onload = function () {
            var customers = JSON.parse(xhttp.responseText);
            for (let i = 0; i < customers.length; i++) {
                if (customers[i].customerId == document.getElementById('customerChoose').value) {
                    let customerToInsert = {
                        customerID: customers[i].customerId,
                        name: customers[i].name,
                        phone: customers[i].phone,
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
                    xhttpPost.open("POST", "https://localhost:5002/api/Order/");
                    xhttpPost.onload = function () {
                        if (xhttpPost.status == 200) {
                            document.location = 'add-product.html';
                        } else {
                            if (confirm("Order don't add. Close?")) {
                                document.location = 'add-product.html';
                            }
                        }
                    }
                    xhttpPost.setRequestHeader('Content-Type', 'application/json');
                    xhttpPost.send(itemToInsertJson);
                }
            }
        }
        xhttp.send();
    }
}

function CheckValidationData() {
    var flag = true;

    var form = document.querySelector('.add-page')
    var fields = form.querySelectorAll('.field')
    var errors = form.querySelectorAll('.error')

    for (var i = 0; i < errors.length; i++) {
        errors[i].remove()
    }

    for (var i = 0; i < fields.length; i++) {
        if (!fields[i].value) {
            var error = document.createElement('div')
            error.className = 'error'
            error.style.color = 'red'
            error.innerHTML = 'Cannot be blank'
            form[i].parentElement.insertBefore(error, fields[i])
            flag = false
        }
    }

    return flag
}

