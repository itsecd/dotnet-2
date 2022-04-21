function addCustomer() {
    if (CheckValidationData()) {
        let itemToInsert = {
            name: document.getElementById('name').value,
            phone: document.getElementById('telephone').value,
            address: document.getElementById('address').value
        };
        let itemToInsertJson = JSON.stringify(itemToInsert);
        let xhttp = new XMLHttpRequest();
        xhttp.open("POST", "https://localhost:5002/api/Customer/");
        xhttp.setRequestHeader('Content-Type', 'application/json');
        xhttp.onload = function () {
            if (xhttp.status == 200) {
                document.location = 'start-customer.html';
            } else {
                if (confirm("Customer don't add. Close?")) {
                    document.location = 'start-customer.html';
                }
            }
        }
        xhttp.send(itemToInsertJson);
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