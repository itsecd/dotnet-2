function addCustomer() {
    if (CheckValidationData()) {
        const itemToInsert = {
            name: document.getElementById('name').value,
            phone: document.getElementById('telephone').value,
            address: document.getElementById('address').value
        };
        const itemToInsertJson = JSON.stringify(itemToInsert);
        const xhttp = new XMLHttpRequest();
        xhttp.open('POST', url + 'api/Customer/');
        xhttp.setRequestHeader('Content-Type', 'application/json');
        xhttp.onload = function () {
            if (xhttp.status == 200) {
                document.location = 'start-customer.html';
            } else {
                if (confirm('Customer dont add. Close?')) {
                    document.location = 'start-customer.html';
                }
            }
        }
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