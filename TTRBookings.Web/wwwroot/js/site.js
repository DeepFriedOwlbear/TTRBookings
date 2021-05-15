// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your Javascript code.

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