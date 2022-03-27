$.validator.addMethod('unlike',
    function (value, element, params) {
        var propertyValue = $(params[0]).val();
        var dependentPropertyValue = $(params[1]).val();
        return propertyValue !== dependentPropertyValue;
    });

$.validator.unobtrusive.adapters.add('unlike',
    ['property'],
    function (options) {
        var element = $(options.form).find('#' + options.params['property'])[0];
        options.rules['unlike'] = [element, options.element];
        options.messages['unlike'] = options.message;
    });
