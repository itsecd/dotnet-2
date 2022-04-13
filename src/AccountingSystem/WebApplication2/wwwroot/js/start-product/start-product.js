window.onload = function() {
    selectAllItems();
}

function selectAllItems() {
	var xhttp = new XMLHttpRequest();
	xhttp.open("GET", "api/Product");
	xhttp.onload = function() {
        var products = JSON.parse(xhttp.responseText);
        var rows = "<tr>" +
            "<td>" + 'Id' + "</td>" +
            "<td>" + 'Name' + "</td>" +
            "<td>" + 'Price' + "</td>" +
            "<td>" + 'Date' + "</td>" +
            "</tr>";
        for (i = 0; i < products.length; i++) {
            rows +=
                "<tr>" +
                "<td>" + products[i].ProductId + "</td>" +
                "<td>" + products[i].Name + "</td>" +
                "<td>" + products[i].Price + "</td>" +
                "<td>" + products[i].Date + "</td>" +
                "</tr>";
        }
        document.getElementById("productTable").innerHTML = rows;
    }
	xhttp.send();
}

function changeProduct() {
    var id = document.getElementById('updateProductChoose').value
    var itemToUpdate = {
        name: document.getElementById('updateName').value,
        price: document.getElementById('updatePrice').value,
        date: document.getElementById('updateDate').value
	};
    var itemToUpdateJson = JSON.stringify(itemToUpdate);
    var xhttp = new XMLHttpRequest();
    xhttp.open('PUT', 'api/Product/' + id);
    xhttp.setRequestHeader('Content-Type', 'application/json');
    xhttp.onload = function() {
        selectAllItems();
        getProductsToChange();
    }
    xhttp.send(itemToUpdateJson);
}

function deleteProduct() {
    var id = document.getElementById('deleteProductChoose').value;
    var xhttp = new XMLHttpRequest();
    xhttp.open('DELETE', 'api/Product/' + id);
    xhttp.onload = function() {
        selectAllItems();
        getProductsToDelete();
    }
    xhttp.send();
}

function getProductsToChange() {
    var xhttp = new XMLHttpRequest();
    xhttp.open("GET", "api/Product");
    xhttp.onload = function() {
        var products = JSON.parse(xhttp.responseText);
        let rows = '';
        for (let i = 0; i < products.length; i++) {
            rows +=
                "<option value = " + customers[i].ProductId + ">" +
                products[i].Name + "</option>";
        }
        document.getElementById("updateProductChoose").innerHTML = rows;
    }
    xhttp.send();
}

function getProductsToDelete() {
    var xhttp = new XMLHttpRequest();
    xhttp.open("GET", "api/Product");
    xhttp.onload = function() {
        var products = JSON.parse(xhttp.responseText);
        let rows = '';
        for (let i = 0; i < products.length; i++) {
            rows +=
                "<option value = " + products[i].customerID + ">" +
                products[i].name + "</option>";
        }
        document.getElementById("deleteProductChoose").innerHTML = rows;
    }
    xhttp.send();
}
