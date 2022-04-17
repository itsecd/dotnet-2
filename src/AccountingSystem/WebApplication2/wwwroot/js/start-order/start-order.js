window.onload = function() {
    selectAllItems();
}

function selectAllItems() {
	var xhttp = new XMLHttpRequest();
	xhttp.open("GET", "../api/Order/");
	xhttp.onload = function() {
        var orders = JSON.parse(xhttp.responseText);
        var rows = "<tr>" +
            "<td>" + 'ID' + "</td>" +
            "<td>" + 'Customer' + "</td>" +
            "<td>" + 'Price' + "</td>" +
            "<td>" + 'Status' + "</td>" +
            "<td>" + 'Date' + "</td>" +
            "<td><table><tr>" + 'Products' + "</tr>" +
            "<tr>" +
            "<td>" + 'ID' + "</td>" +
            "<td>" + 'Name' + "</td>" +
            "<td>" + 'Price' + "</td>" +
            "<td>" + 'Date' + "</td>"+
            "</tr></table></td>" +
            "</tr>";
        for (i = 0; i < orders.length; i++) {
            rows +=
                "<tr>" +
                "<td>" + orders[i].orderId + "</td>" +
                "<td>" + orders[i].customer.name + "</td>" +
                "<td>" + orders[i].price + "</td>" +
                "<td>" + orders[i].status + "</td>" +
                "<td>" + orders[i].date + "</td>";
            rows += "<td><table>"
            for (j = 0; j < orders[i].products.length; j++) {
                rows +=
                    "<tr>" +
                    "<td>" + orders[i].products[j].productId + "</td>" +
                    "<td>" + orders[i].products[j].name + "</td>" +
                    "<td>" + orders[i].products[j].price + "</td>" +
                "<td>" + orders[i].products[j].date + "</td></tr > ";
            }
            rows += "</table></td></tr>";
        }
        document.getElementById("orderTable").innerHTML = rows;
    }
	xhttp.send();
}

function changeOrder() {
    var xhttp = new XMLHttpRequest();
    xhttp.open("GET", "rest/customers/");
    xhttp.onload = function() {
        var customers = JSON.parse(xhttp.responseText);
        for (let i = 0; i < customers.length; i++) {
            if(customers[i].customerID == document.getElementById('updateCustomerChoose').value){
                let CustomerToUpdate = {
                    customerID: customers[i].customerID,
                    name: customers[i].name,
                    phoneNumber: customers[i].phoneNumber,
                    address: customers[i].address
                };
                var id = document.getElementById('updateOrderChoose').value
                var itemToUpdate = {
                    customer: CustomerToUpdate,
                    orderDate: document.getElementById('updateDate').value,
                    orderPrice: document.getElementById('updatePrice').value
                };
                var itemToUpdateJson = JSON.stringify(itemToUpdate);
                var xhttpPut = new XMLHttpRequest();
                xhttpPut.open('PUT', 'rest/orders/' + id);
                xhttpPut.setRequestHeader('Content-Type', 'application/json');
                xhttpPut.onload = function() {
                    selectAllItems();
                    getOrdersToChange();
                }
                xhttpPut.send(itemToUpdateJson);
            }
        }
    }
    xhttp.send();
}

function deleteOrder() {
    var id = document.getElementById('deleteChoose').value;
    var xhttp = new XMLHttpRequest();
    xhttp.open('DELETE', '../api/Order/' + id);
    xhttp.onload = function() {
        selectAllItems();
        getOrdersToDelete();
    }
    xhttp.send();
}

function getAllPrice() {
    var xhttp = new XMLHttpRequest();
    xhttp.open('GET', '../api/Order/all-price');
    xhttp.onload = function () {
        alert("All Price Orders is " + xhttp.responseText)
    }
    xhttp.send();
}


function getCustomersToChange() {
    var xhttp = new XMLHttpRequest();
    xhttp.open("GET", "../api/Customer/");
    xhttp.onload = function() {
        var customers = JSON.parse(xhttp.responseText);
        let rows = '';
        for (let i = 0; i < customers.length; i++) {
            rows +=
                "<option value = " + customers[i].customerID + ">" +
                customers[i].name + "</option>";
        }
        document.getElementById("updateCustomerChoose").innerHTML = rows;
    }
    xhttp.send();
}

function getOrdersToChange() {
    var xhttp = new XMLHttpRequest();
    xhttp.open("GET", "../api/Order/");
    xhttp.onload = function() {
        var orders = JSON.parse(xhttp.responseText);
        let rows = '';
        for (let i = 0; i < orders.length; i++) {
            rows +=
                "<option value = " + orders[i].orderId + ">" +
                orders[i].orderId + "</option>";
        }
        document.getElementById("updateOrderChoose").innerHTML = rows;
    }
    xhttp.send();
}

function getOrdersToDelete() {
    var xhttp = new XMLHttpRequest();
    xhttp.open("GET", "../api/Order/");
    xhttp.onload = function() {
        var orders = JSON.parse(xhttp.responseText);
        let rows = '';
        for (let i = 0; i < orders.length; i++) {
            rows +=
                "<option value = " + orders[i].orderId + ">" +
                orders[i].orderId + "</option>";
        }
        document.getElementById("deleteChoose").innerHTML = rows;
    }
    xhttp.send();
}