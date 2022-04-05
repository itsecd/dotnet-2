window.onload = function() {
    selectAllItems();
}

function selectAllItems() {
	var xhttp = new XMLHttpRequest();
	xhttp.open("GET", "rest/customers/");
	xhttp.onload = function() {
        var customers = JSON.parse(xhttp.responseText);
        var rows = "<tr>" +
            "<td>" + 'ID' + "</td>" +
            "<td>" + 'Name' + "</td>" +
            "<td>" + 'PhoneNumber' + "</td>" +
            "<td>" + 'Address' + "</td>" +
            "</tr>";
        for (i = 0; i < customers.length; i++) {
            rows +=
                "<tr>" +
                "<td>" + customers[i].customerID + "</td>" +
                "<td>" + customers[i].name + "</td>" +
                "<td>" + customers[i].phoneNumber + "</td>" +
                "<td>" + customers[i].address + "</td>" +
                "</tr>";
        }
        document.getElementById("customerTable").innerHTML = rows;
    }
	xhttp.send();
}

function changeCustomer() {
    var id = document.getElementById('updateCustomerChoose').value
    var itemToUpdate = {
        name: document.getElementById('updateName').value,
        phoneNumber: document.getElementById('updatePhone').value,
        address: document.getElementById('updateAddress').value
	};
    var itemToUpdateJson = JSON.stringify(itemToUpdate);
    var xhttp = new XMLHttpRequest();
    xhttp.open('PUT', 'rest/customers/' + id);
    xhttp.setRequestHeader('Content-Type', 'application/json');
    xhttp.onload = function() {
        selectAllItems();
        getCustomersToChange();
    }
    xhttp.send(itemToUpdateJson);
}

function deleteCustomer() {
    var id = document.getElementById('deleteCustomerChoose').value;
    var xhttp = new XMLHttpRequest();
    xhttp.open('DELETE', 'rest/customers/' + id);
    xhttp.onload = function() {
        selectAllItems();
        getCustomersToDelete();
    }
    xhttp.send();
}

function getCustomersToChange() {
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

function getCustomersToDelete() {
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
        document.getElementById("deleteCustomerChoose").innerHTML = rows;
    }
    xhttp.send();
}

function openCustomerFile() {
    var control = document.getElementById("openCustomerFile");
    var file = control.files
    if(file[0].type == 'text/xml'){
        let reader = new FileReader();
        reader.readAsText(file[0]);
        reader.onload = function() {
            var xhttp = new XMLHttpRequest();
            if(confirm("Completely replace the data?")){
                xhttp.open("POST", "rest/xmlCustomer/0");
            }else{
                xhttp.open("POST", "rest/xmlCustomer/1");
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

function xsltCustomer() {
    var xhttp = new XMLHttpRequest();
    xhttp.open("GET", "rest/xmlCustomer/");
    xhttp.onload = function() {
        if(xhttp.responseText == 1){
            window.open("xlst-customer.html")
        }
    }
    xhttp.send();
}
