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
    //Input is now allowed by default and when the picker is open, Input is blocked when the picker is closed
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
async function handleFormSubmit(event, callback) {
    event.preventDefault();

    // This gets the element which the event handler was attached to.
    const form = event.currentTarget;

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
                //console.log('Success:', data);
                //Use Callback function and give it the success state + submitted formData
                callback(data.success, formData);
                //return data;
            })
            .catch((error) => {
                console.error('Error:', error);
            });
    } catch (error) {
        console.error(error);
    }
}