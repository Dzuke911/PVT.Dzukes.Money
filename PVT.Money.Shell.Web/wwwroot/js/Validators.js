﻿$(function () {
    //login validate methods
    jQuery.validator.addMethod('loginvalidateminlength',
        function (value, element) {
            var ret = (value.length >= 8);
            errorClassAdder(element, ret);
            return ret;
         });
    jQuery.validator.addMethod('loginvalidatemaxlength',
        function (value, element) {
            var ret = (value.length <= 16);
            errorClassAdder(element, ret);
            return ret;
        });
    jQuery.validator.addMethod('loginvalidatestart',
        function (value, element) {
            var ret = /^[a-zA-Z0-9].*[a-zA-Z0-9]$/.test(value);
            errorClassAdder(element, ret);
            return ret;
        });
    jQuery.validator.addMethod('loginvalidateallowedcharacters',
        function (value, element) {
            var ret = /^[a-zA-Z0-9_ -]+$/.test(value);
            errorClassAdder(element, ret);
            return ret;
        });
    jQuery.validator.addMethod('loginvalidatespecialcharacters',
        function (value, element) {
            var ret = /^[a-zA-Z0-9]+(?:[_ -]?[a-zA-Z0-9])*$/.test(value);
            errorClassAdder(element, ret);
            return ret;
        });

    //password validate methods
    jQuery.validator.addMethod('passwordvalidateminlength',
        function (value, element) {
            var ret = (value.length >= 8);
            errorClassAdder(element, ret);
            return ret;
        });
    jQuery.validator.addMethod('passwordvalidatemaxlength',
        function (value, element) {
            var ret = (value.length <= 16);
            errorClassAdder(element, ret);
            return ret;
        });
    jQuery.validator.addMethod('passwordallowedcharacters',
        function (value, element) {
            var ret = /^[a-zA-Z0-9]+$/.test(value);
            errorClassAdder(element, ret);
            return ret;
        });
    jQuery.validator.addMethod('passwordcontainslowercase',
        function (value, element) {
            var ret = /^.*[a-z].*$/.test(value);
            errorClassAdder(element, ret);
            return ret;
        });
    jQuery.validator.addMethod('passwordcontainsuppercase',
        function (value, element) {
            var ret = /^.*[A-Z].*$/.test(value);
            errorClassAdder(element, ret);
            return ret;
        });
    jQuery.validator.addMethod('passwordcontainsnumeric',
        function (value, element) {
            var ret = /^.*[0-9].*$/.test(value);
            errorClassAdder(element, ret);
            return ret;
        });

    //compareFalse validate methods
    jQuery.validator.addMethod('comparefalseerrormessage',
        function (value, element) {
            var compareString = $("#TransactToAccount_AccountFromName option:selected").attr("value");
            var ret = (compareString != value);
            errorAccClassAdder(element, ret);
            return ret;
        });

     //compareFalse validate adapters
    jQuery.validator.unobtrusive.adapters.addBool('comparefalseerrormessage');

    //login validate adapters
    jQuery.validator.unobtrusive.adapters.addBool('loginvalidateminlength');
    jQuery.validator.unobtrusive.adapters.addBool('loginvalidatemaxlength');
    jQuery.validator.unobtrusive.adapters.addBool('loginvalidatestart');
    jQuery.validator.unobtrusive.adapters.addBool('loginvalidateallowedcharacters');
    jQuery.validator.unobtrusive.adapters.addBool('loginvalidatespecialcharacters');

    //password validate adapters
    jQuery.validator.unobtrusive.adapters.addBool('passwordvalidateminlength');
    jQuery.validator.unobtrusive.adapters.addBool('passwordvalidatemaxlength');
    jQuery.validator.unobtrusive.adapters.addBool('passwordallowedcharacters');
    jQuery.validator.unobtrusive.adapters.addBool('passwordcontainslowercase');
    jQuery.validator.unobtrusive.adapters.addBool('passwordcontainsuppercase');
    jQuery.validator.unobtrusive.adapters.addBool('passwordcontainsnumeric');

    //jQuery.validator.unobtrusive.adapters.add('passwordvalid',
    //    function (options) {
    //        var element = $(options.form).find('select#Password')[0];
    //        options.rules['passwordvalid'] = [element];
    //        options.messages['passwordvalid'] = options.message;
    //    });
}(jQuery));

 function errorClassAdder(element, bool) {
     if (bool == true) {
         $(element).parent().removeClass("has-error");
     }
     else {
         $(element).parent().addClass("has-error");
     }
 }

//
// add errors to accounts select lists
//
function errorAccClassAdder(element, bool) {
    if (bool == true) {
        $("#TransactToAccount_AccountFromName").removeClass("has-error");
        $("#TransactToAccount_AccountToName").removeClass("has-error");
    }
    else {
        $("#TransactToAccount_AccountFromName").addClass("has-error");
        $("#TransactToAccount_AccountToName").addClass("has-error");
    }
}
