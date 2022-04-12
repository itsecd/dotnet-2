function openChangePanel() {
    document.getElementById("changeDiv").style.display = "block";
}

function closeChangePanel() {
    document.getElementById("updateDate").value = "";
    document.getElementById("updateDate").value = "updatePrice";
    document.getElementById("changeDiv").style.display = 'none';
}

function openDeletePanel() {
    document.getElementById("deleteDiv").style.display = "block";
}

function closeDeletePanel() {
    document.getElementById("deleteDiv").style.display = "none";
}

function openOpenPanel() {
    document.getElementById("openDiv").style.display = "block";
}

function closeOpenPanel() {
    document.getElementById("openOrderFile").value = "";
    document.getElementById("openDiv").style.display = "none";
}
