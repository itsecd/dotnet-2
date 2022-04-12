window.onload = function() {
    selectAllItems();
}

function selectAllItems() {
	var xhttp = new XMLHttpRequest();
	xhttp.open("GET", "rest/orders/");
	xhttp.onload = function() {
        var orders = JSON.parse(xhttp.responseText);
        var rows = "<tr>" +
            "<td>" + 'ID' + "</td>" +
            "<td>" + 'Customer' + "</td>" +
            "<td>" + 'Date' + "</td>" +
            "<td>" + 'Price' + "</td>" +
            "</tr>";
        for (i = 0; i < orders.length; i++) {
            rows +=
                "<tr>" +
                "<td>" + orders[i].orderID + "</td>" +
                "<td>" + orders[i].customer.customerID + "</td>" +
                "<td>" + orders[i].orderDate + "</td>" +
                "<td>" + orders[i].orderPrice + "</td>" +
                "</tr>";
        }
        console.log(rows)
        console.log(orders)
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
    xhttp.open('DELETE', 'rest/orders/' + id);
    xhttp.onload = function() {
        selectAllItems();
        getOrdersToDelete();
    }
    xhttp.send();
}

function getCustomers() {
    var xhttp = new XMLHttpRequest();
    xhttp.open("GET", "rest/customers/");
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
    xhttp.open("GET", "rest/orders/");
    xhttp.onload = function() {
        var orders = JSON.parse(xhttp.responseText);
        let rows = '';
        for (let i = 0; i < orders.length; i++) {
            rows +=
                "<option value = " + orders[i].orderID + ">" +
                orders[i].orderID + "</option>";
        }
        document.getElementById("updateOrderChoose").innerHTML = rows;
    }
    xhttp.send();
}

function getOrdersToDelete() {
    var xhttp = new XMLHttpRequest();
    xhttp.open("GET", "rest/orders/");
    xhttp.onload = function() {
        var orders = JSON.parse(xhttp.responseText);
        let rows = '';
        for (let i = 0; i < orders.length; i++) {
            rows +=
                "<option value = " + orders[i].orderID + ">" +
                orders[i].orderID + "</option>";
        }
        document.getElementById("deleteChoose").innerHTML = rows;
    }
    xhttp.send();
}


function openOrderFile() {
    var control = document.getElementById("openOrderFile");
    var file = control.files
    if(file[0].type == 'text/xml'){
        let reader = new FileReader();
        reader.readAsText(file[0]);
        reader.onload = function() {
            var xhttp = new XMLHttpRequest();
            if(confirm("Completely replace the data?")){
                xhttp.open("POST", "rest/xmlOrder/0");
            }else{
                xhttp.open("POST", "rest/xmlOrder/1");
            }
            xhttp.onload = function() {
                if(xhttp.responseText == 1){
                    selectAllItems();
                }else {
                    alert("File is incorrect!");
                }
            }
            xhttp.send(reader.result);
        };
        reader.onerror = function() {
            console.log(reader.error);
        };
    }else{
        alert("File is not XML");
    }
}

function xsltOrder() {
    var xhttp = new XMLHttpRequest();
    xhttp.open("GET", "rest/xmlOrder/");
    xhttp.onload = function() {
        if(xhttp.responseText == 1){
            window.open("xlst-order.html")
        }
    }
    xhttp.send();

}