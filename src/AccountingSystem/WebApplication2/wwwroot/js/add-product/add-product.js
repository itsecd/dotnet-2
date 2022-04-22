window.onload = function () {
    getOrders();
    document.getElementById('date').valueAsDate = new Date();
}

function selectAllProducts() {
    const xhttp = new XMLHttpRequest();
    xhttp.open('GET', url + 'api/Order/' + document.getElementById('orderChoose').value + '/products');
    xhttp.onload = function () {
        const products = JSON.parse(xhttp.responseText);
        let rows = '<tr>' +
            '<td>' + 'ID' + '</td>' +
            '<td>' + 'Name' + '</td>' +
            '<td>' + 'Price' + '</td>' +
            '<td>' + 'Date' + '</td>' +
            '</tr>';
        for (i = 0; i < products.length; i++) {
            rows +=
                '<tr>' +
                '<td>' + products[i].productId + '</td>' +
                '<td>' + products[i].name + '</td>' +
                '<td>' + products[i].price + '</td>' +
                '<td>' + new Date(products[i].date).toDateString() + '</td></tr > ';
        }
        document.getElementById('productTable').innerHTML = rows;
    }
    xhttp.send();
}

function getOrders() {
    const xhttp = new XMLHttpRequest();
    xhttp.open('GET', url + 'api/Order/');
    xhttp.onload = function () {
        const orders = JSON.parse(xhttp.responseText);
        let rows = "<option value = ''>--</option>";
        for (let i = 0; i < orders.length; i++) {
            rows +=
                '<option value = ' + orders[i].orderId + '>' +
                orders[i].customer.name + '(' + new Date(orders[i].date).toDateString() + ')' + '</option>';
        }
        document.getElementById('orderChoose').innerHTML = rows;
        selectAllProducts();
    }
    xhttp.send();
}

function addProduct() {
    if (CheckValidationData()) {
        const itemToInsert = {
            name: document.getElementById('name').value,
            price: document.getElementById('price').value,
            date: document.getElementById('date').value
        };
        const itemToInsertJson = JSON.stringify(itemToInsert);
        const xhttp = new XMLHttpRequest();
        xhttp.onload = function () {
            if (xhttp.status == 200) {
                selectAllProducts();
            } else {
                alert('Customer dont add');
            }
        }
        xhttp.open('POST', url + 'api/Order/' + document.getElementById('orderChoose').value + '/products');
        xhttp.setRequestHeader('Content-Type', 'application/json');
        xhttp.send(itemToInsertJson);
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