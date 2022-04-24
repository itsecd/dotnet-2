window.onload = function() {
    getCustomers();
    getStatus();
    document.getElementById('date').valueAsDate = new Date();
}

function getCustomers() {
    const xhttp = new XMLHttpRequest();
    xhttp.open('GET', url + 'api/Customer/');
    xhttp.onload = function() {
        const customers = JSON.parse(xhttp.responseText);
        let rows = '';
        for (let i = 0; i < customers.length; i++) {
            rows +=
                '<option value = ' + customers[i].customerId + '>' +
                customers[i].name + '</option>';
        }
        document.getElementById('customerChoose').innerHTML = rows;
    }
    xhttp.send();
}


function getStatus() {
    let rows = '<option value = "">--</option>' +
        '<option value = "0">0. Initial</option>' +
        '<option value = "1">1. In Process</option>' +
        '<option value = "2">2. Referred</option>' +
        '<option value = "3">3. Partly Serviced</option>' +
        '<option value = "4">4. Quote Finished</option>' +
        '<option value = "5">5. Waiting for Customer</option>' +
        '<option value = "6">6. Spare Part Ordered</option>' +
        '<option value = "7">7. Spare Part Received</option>' +
        '<option value = "8">8. Finished</option>';
    document.getElementById('status').innerHTML = rows;
}

function addOrder() {
    if (CheckValidationData()) {
        const xhttp = new XMLHttpRequest();
        xhttp.open('GET', url + 'api/Customer');
        xhttp.onload = function () {
            const customers = JSON.parse(xhttp.responseText);
            for (let i = 0; i < customers.length; i++) {
                if (customers[i].customerId === document.getElementById('customerChoose').value) {
                    const customerToInsert = {
                        customerID: customers[i].customerId,
                        name: customers[i].name,
                        phone: customers[i].phone,
                        address: customers[i].address
                    };
                    const itemToInsert = {
                        customer: customerToInsert,
                        date: document.getElementById('date').value,
                        status: document.getElementById('status').value,
                        products: []
                    };
                    const itemToInsertJson = JSON.stringify(itemToInsert);
                    const xhttpPost = new XMLHttpRequest();
                    xhttpPost.open('POST', url + 'api/Order/');
                    xhttpPost.onload = function () {
                        if (xhttpPost.status === 200) {
                            document.location = 'start-order.html';
                        } else {
                            if (confirm('Order dont add. Close?')) {
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
        const xhttp = new XMLHttpRequest();
        xhttp.open('GET', url + 'api/Customer');
        xhttp.onload = function () {
            let customers = JSON.parse(xhttp.responseText);
            for (let i = 0; i < customers.length; i++) {
                if (customers[i].customerId === document.getElementById('customerChoose').value) {
                    const customerToInsert = {
                        customerID: customers[i].customerId,
                        name: customers[i].name,
                        phone: customers[i].phone,
                        address: customers[i].address
                    };
                    const itemToInsert = {
                        customer: customerToInsert,
                        date: document.getElementById('date').value,
                        status: document.getElementById('status').value,
                        products: []
                    };
                    const itemToInsertJson = JSON.stringify(itemToInsert);
                    const xhttpPost = new XMLHttpRequest();
                    xhttpPost.open('POST', url + 'api/Order/');
                    xhttpPost.onload = function () {
                        if (xhttpPost.status === 200) {
                            document.location = 'add-product.html';
                        } else {
                            if (confirm('Order dont add. Close?')) {
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
    let flag = true;

    const form = document.querySelector('.add-page');
    const fields = form.querySelectorAll('.field');
    const errors = form.querySelectorAll('.error');

    for (let i = 0; i < errors.length; i++) {
        errors[i].remove();
    }

    for (let i = 0; i < fields.length; i++) {
        if (!fields[i].value) {
            const error = document.createElement('div');
            error.className = 'error';
            error.style.color = 'red';
            error.innerHTML = 'Cannot be blank';
            form[i].parentElement.insertBefore(error, fields[i]);
            flag = false;
        }
    }

    return flag;
}

