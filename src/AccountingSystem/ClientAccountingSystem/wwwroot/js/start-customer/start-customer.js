window.onload = function() {
    selectAllItems();
}

function selectAllItems() {
	const xhttp = new XMLHttpRequest();
    xhttp.open('GET', url + 'api/Customer/');
	xhttp.onload = function() {
        const customers = JSON.parse(xhttp.responseText);
        let rows = '<tr>' +
            '<td>' + 'Id' + '</td>' +
            '<td>' + 'Name' + '</td>' +
            '<td>' + 'Phone' + '</td>' +
            '<td>' + 'Address' + '</td>' +
            '</tr>';
        for (i = 0; i < customers.length; i++) {
            rows +=
                '<tr>' +
                '<td>' + customers[i].customerId + '</td>' +
                '<td>' + customers[i].name + '</td>' +
                '<td>' + customers[i].phone + '</td>' +
                '<td>' + customers[i].address + '</td>' +
                '</tr>';
        }
        document.getElementById('customerTable').innerHTML = rows;
    }
	xhttp.send();
}

function changeCustomer() {
    if (CheckValidationData('.changeDiv')) {
        const id = document.getElementById('updateCustomerChoose').value;
        const itemToUpdate = {
            name: document.getElementById('updateName').value,
            phone: document.getElementById('updatePhone').value,
            address: document.getElementById('updateAddress').value
        };
        const itemToUpdateJson = JSON.stringify(itemToUpdate);
        const xhttp = new XMLHttpRequest();
        xhttp.open('PUT', url + 'api/Customer/' + id);
        xhttp.setRequestHeader('Content-Type', 'application/json');
        xhttp.onload = function () {
            if (xhttp.status === 200) {
                selectAllItems();
                getCustomersToChange();
            } else {
                alert('Customer dont change');
            }

        }
        xhttp.send(itemToUpdateJson);
    }
}

function deleteCustomer() {
    if (CheckValidationData('.deleteDiv')) {
        const id = document.getElementById('deleteCustomerChoose').value;
        const xhttp = new XMLHttpRequest();
        xhttp.open('DELETE', url + 'api/Customer/' + id);
        xhttp.onload = function () {
            if (xhttp.status === 200) {
                selectAllItems();
                getCustomersToDelete();
            } else {
                alert('Customer dont delete');
            }
        }
        xhttp.send();
    }
}

function getCustomersToChange() {
    getCustomers('updateCustomerChoose');
}

function getCustomersToDelete() {
    getCustomers('deleteCustomerChoose');
}

function getCustomers(customerChooseName) {
    const xhttp = new XMLHttpRequest();
    xhttp.open('GET', url + 'api/Customer/');
    xhttp.onload = function () {
        const customers = JSON.parse(xhttp.responseText);
        let rows = '';
        for (let i = 0; i < customers.length; i++) {
            rows +=
                '<option value = ' + customers[i].customerId + '>' +
            customers[i].customerId + '</option>';
        }
        document.getElementById(customerChooseName).innerHTML = rows;
    }
    xhttp.send();
}

function CheckValidationData(divName) {
    let flag = true;

    const form = document.querySelector('.start-page');
    const div = form.querySelector(divName);
    const fields = div.querySelectorAll('.field');
    const errors = div.querySelectorAll('.error');

    for (let i = 0; i < errors.length; i++) {
        errors[i].remove();
    }

    for (let i = 0; i < fields.length; i++) {
        if (!fields[i].value) {
            const error = document.createElement('div');
            error.className = 'error';
            error.style.color = 'red';
            error.innerHTML = 'Cannot be blank';
            fields[i].parentElement.insertBefore(error, fields[i]);
            flag = false;
        }
    }

    return flag;
}
