//toastr config
toastr.options = {
    "closeButton": true,
    "newestOnTop": true,
    "progressBar": false,
    "positionClass": "toast-top-center",
    "preventDuplicates": false,
    "onclick": null,
    "showDuration": "300",
    "hideDuration": "1000",
    "timeOut": "0",
    "extendedTimeOut": "0",
    "showEasing": "swing",
    "hideEasing": "linear",
    "showMethod": "fadeIn",
    "hideMethod": "fadeOut"
}

//flatpickr config
flatpickr.setDefaults({
    altInput: true,
    enableTime: true,
    time_24hr: true,
    dateFormat: "d.m.Y H:i:S",
    altFormat: "D., d-m-Y H:i",
    minuteIncrement: 15,
    //Flatpickr blocks input by default, but client-side data validation needs the input allowed to validate
    //Input is now allowed by default when the picker is open, Input is blocked when the picker is closed
    allowInput: true,
    onOpen: function (selectedDates, dateStr, instance) {
        $(instance.altInput).prop('readonly', true);
    },
    onClose: function (selectedDates, dateStr, instance) {
        $(instance.altInput).prop('readonly', false);
        $(instance.altInput).blur();
    }
});

////function to round date for flatpickr
//var today = new Date();
//function getRoundedDate(minutes, d = new Date(), offsetMinutes = 0) {
//    let ms = 1000 * 60 * minutes; //minutes to milliseconds
//    let roundedDate = new Date(Math.round(d.getTime() / ms) * ms + offsetMinutes * 60000); //round Date and add offsetMinutes
//    return roundedDate
//}

// Bootstrap Client-Side Validation
(function () {
    'use strict'
    // Fetch all the forms to apply custom Bootstrap validation styles to
    var forms = document.querySelectorAll('.needs-validation')
    // Loop over them and prevent submission
    Array.prototype.slice.call(forms)
        .forEach(function (form) {
            form.addEventListener('submit', function (event) {
                if (!form.checkValidity()) {
                    event.preventDefault()
                    event.stopPropagation()
                }
                form.classList.add('was-validated')
            }, false)
        })
})();

// Event handler for a form submit event.
async function handleFormSubmit(event, callback, redirectString) {
    event.preventDefault();
    // This gets the element which the event handler was attached to.
    const form = event.currentTarget;
    const myFormId = event.target.id;

    // This takes the API URL from the form's `action` attribute.
    const url = form.action;
    try {
        // This takes all the fields in the form and makes their values available through a `FormData` instance.
        const formData = new FormData(form);
        const responseData = await fetch(url,
            {
                method: 'POST', // or 'PUT'
                headers: {
                    'Content-Type': 'application/json',
                    "Accept": "application/json"
                },
                body: JSON.stringify(Object.fromEntries(formData)),
            })
            .then(response => response.json())
            .then(data => {
                //Use Callback function
                if (redirectString != undefined) {
                    //send success state and redirect string
                    callback(data, redirectString);
                } else {
                    //send success state and form data
                    callback(data, myFormId);
                }
            })
            .catch((error) => {
                console.error('Error:', error);
            });
    } catch (error) {
        console.error(error);
    }
}

//Selects all deleteEntry forms on the site and attaches the handleFormSubmit function
function AddEventListenerToDeleteEntries() {
    document.querySelectorAll('[id^="deleteEntry_"]')
        .forEach((element) => {
            element.addEventListener("submit", function () {
                handleFormSubmit(event, RemoveFromUI);
            });
        });
}

//Callback function that checks if handleFormSubmit was a success. Redirects or shows toastr errors depending on the success state
function SuccessRedirect(data, redirectString) {
    if (data.success) {
        //if success, redirect to specified page
        window.location.replace(redirectString);
    }
    else {
        //if failure, display toastr errors
        let toastrErrors = JSON.parse(data.toastrJSON);
        for (let [key, value] of Object.entries(toastrErrors)) {
            toastr.error(value, key);
        }
    }
}

//Callback function that removes a row in the Index lists (Bookings/Index, Managers/Index, etc)
function RemoveFromUI(data, myFormId) {
    if (data.success) {
        let form = document.getElementById(myFormId);
        var row = form.parentNode.parentNode;
        row.parentNode.removeChild(row);
    }
    else {
        toastr.error("Entry could not be deleted.", "Warning");
    }
}

//unlock Fieldset function for locked form fields
function UnlockFieldset(fieldName, originalName) {
    let unlockButton = document.getElementById("unlockFormButton");
    let fieldset = document.getElementById("FormFieldset");

    //if fieldset is disabled:
    //lock fieldset, change look of fieldset, rename and change look of edit button to cancel button, show save changes button
    if (fieldset.disabled == true) {
        fieldset.disabled = false;
        fieldset.className = "p-3 mb-2";
        unlockButton.textContent = "Cancel";
        unlockButton.className = "btn btn-danger";
        document.getElementById("submitButton").hidden = false;
    }
    //if fieldset is active:
    //restore fieldset to original look and restore original values
    else {
        fieldset.disabled = true;
        fieldset.className = "p-3 mb-2 bg-secondary text-white";
        unlockButton.textContent = "Edit";
        unlockButton.className = "btn btn-secondary";
        document.getElementById("submitButton").hidden = true;
        document.getElementById(fieldName).value = originalName;
    }
}