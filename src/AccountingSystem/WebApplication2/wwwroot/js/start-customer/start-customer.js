window.onload = function() {
    selectAllItems();
}

function selectAllItems() {
	var xhttp = new XMLHttpRequest();
    xhttp.open("GET", "https://localhost:5002/api/Customer/");
	xhttp.onload = function() {
        var customers = JSON.parse(xhttp.responseText);
        var rows = "<tr>" +
            "<td>" + 'Id' + "</td>" +
            "<td>" + 'Name' + "</td>" +
            "<td>" + 'Phone' + "</td>" +
            "<td>" + 'Address' + "</td>" +
            "</tr>";
        for (i = 0; i < customers.length; i++) {
            rows +=
                "<tr>" +
                "<td>" + customers[i].customerId + "</td>" +
                "<td>" + customers[i].name + "</td>" +
                "<td>" + customers[i].phone + "</td>" +
                "<td>" + customers[i].address + "</td>" +
                "</tr>";
        }
        document.getElementById("customerTable").innerHTML = rows;
    }
	xhttp.send();
}

function changeCustomer() {
    if (CheckValidationData('.changeDiv')) {
        var id = document.getElementById('updateCustomerChoose').value
        var itemToUpdate = {
            name: document.getElementById('updateName').value,
            phone: document.getElementById('updatePhone').value,
            address: document.getElementById('updateAddress').value
        };
        var itemToUpdateJson = JSON.stringify(itemToUpdate);
        var xhttp = new XMLHttpRequest();
        xhttp.open('PUT', 'https://localhost:5002/api/Customer/' + id);
        xhttp.setRequestHeader('Content-Type', 'application/json');
        xhttp.onload = function () {
            if (xhttp.status == 200) {
                selectAllItems();
                getCustomersToChange();
            } else {
                alert("Customer don't change");
            }

        }
        xhttp.send(itemToUpdateJson);
    }
}

function deleteCustomer() {
    if (CheckValidationData('.deleteDiv')) {
        var id = document.getElementById('deleteCustomerChoose').value;
        var xhttp = new XMLHttpRequest();
        xhttp.open('DELETE', 'https://localhost:5002/api/Customer/' + id);
        xhttp.onload = function () {
            if (xhttp.status == 200) {
                selectAllItems();
                getCustomersToDelete();
            } else {
                alert("Customer don't delete");
            }
        }
        xhttp.send();
    }
}

function getCustomersToChange() {
    getCustomers("updateCustomerChoose");
}

function getCustomersToDelete() {
    getCustomers("deleteCustomerChoose");
}

function getCustomers(customerChooseName) {
    var xhttp = new XMLHttpRequest();
    xhttp.open("GET", "https://localhost:5002/api/Customer/");
    xhttp.onload = function () {
        var customers = JSON.parse(xhttp.responseText);
        let rows = '';
        for (let i = 0; i < customers.length; i++) {
            rows +=
                "<option value = " + customers[i].customerId + ">" +
            customers[i].customerId + "</option>";
        }
        document.getElementById(customerChooseName).innerHTML = rows;
    }
    xhttp.send();
}

function CheckValidationData(divName) {
    var flag = true;

    var form = document.querySelector('.start-page')
    var div = form.querySelector(divName)
    var fields = div.querySelectorAll('.field')
    var errors = div.querySelectorAll('.error')

    for (var i = 0; i < errors.length; i++) {
        errors[i].remove()
    }

    for (var i = 0; i < fields.length; i++) {
        if (!fields[i].value) {
            var error = document.createElement('div')
            error.className = 'error'
            error.style.color = 'red'
            error.innerHTML = 'Cannot be blank'
            fields[i].parentElement.insertBefore(error, fields[i])
            flag = false
        }
    }

    return flag
}
