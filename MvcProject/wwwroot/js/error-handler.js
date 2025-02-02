// error-handler.js
const ErrorHandler = {
    init() {
        // Configure Toastr
        toastr.options = {
            "closeButton": true,
            "progressBar": true,
            "positionClass": "toast-top-right",
            "timeOut": "5000"
        };

        // Add global AJAX error handler
        $(document).ajaxError(function (event, jqXHR) {
            let message = 'An error occurred';

            if (jqXHR.responseJSON) {
                message = jqXHR.responseJSON.message || jqXHR.responseJSON.Message;
            }

            switch (jqXHR.status) {
                case 400:
                    toastr.warning(message);
                    break;
                case 401:
                    toastr.error('Unauthorized access. Please log in again.');
                    break;
                case 403:
                    toastr.error('Access forbidden.');
                    break;
                case 404:
                    toastr.warning('Resource not found.');
                    break;
                default:
                    toastr.error(message || 'A server error occurred. Please try again later.');
            }
        });

        // Add fetch error handler
        window.addEventListener('unhandledrejection', function (event) {
            if (event.reason instanceof Error) {
                toastr.error(event.reason.message);
            }
        });
    },

    // Helper methods for different types of notifications
    success(message) {
        toastr.success(message);
    },

    error(message) {
        toastr.error(message);
    },

    warning(message) {
        toastr.warning(message);
    },

    info(message) {
        toastr.info(message);
    }
};