class BootstrapValidator5 {
    settings = {
        form: Element,
        inputs: [Object]
    }

    constructor(formQuerySelect) {
        const validateAttrPrefix = "validate-";
        let isFirst = true;

        let eform = document.querySelector(formQuerySelect);
        if (eform === null) {
            console.error("the selector is incorrect.");
        }
        else if (eform.tagName.toLowerCase() !== 'form') {
            console.error("The element is not form.");
        }

        eform.setAttribute("novalidate", "");

        let einputs = eform.getElementsByTagName("input");
        let selectInputs = [];
        let inputEvents = [];
        for (let i in einputs) {
            let input = einputs[i];
            let needValidate = false;

            if (input.classList == null || input.classList.length == 0)
                continue;

            input.classList.forEach(function (className) {
                if (className == "need-validate") {
                    needValidate = true;
                }
            });

            if (needValidate) {
                //executing validate
                let executingFnName = input.getAttribute(validateAttrPrefix + "on-executing");
                let fn = window[executingFnName];
                let jsValidateMsg = "";
                let onInputEvent = undefined;
                if (typeof fn !== "undefined") {
                    onInputEvent = function (e) {
                        let errorMsg = fn(input);
                        if (typeof errorMsg === "boolean") {
                            if (errorMsg === false) {
                                jsValidateMsg = " ";
                            }
                        }
                        else if (typeof errorMsg === "string" && errorMsg.length != 0) {
                            jsValidateMsg = " ";
                            input.parentElement.getElementsByClassName("invalid-feedback")[0].innerHTML = errorMsg;
                        }
                        else {
                            jsValidateMsg = "";
                        }

                        input.setCustomValidity(jsValidateMsg);
                    };
                    eform.addEventListener("input", onInputEvent);
                }

                selectInputs.push(input);
                inputEvents.push(onInputEvent);
            }
        }

        eform.addEventListener("submit", function (e) {
            //Call event for validate when it's in first time.
            if (isFirst) {
                isFirst = false;

                for (let i in selectInputs) {
                    let fn = inputEvents[i];
                    if (typeof fn !== "undefined") {
                        let input = selectInputs[i];
                        inputEvents[i](input);
                    }
                }
            }

            if (!eform.checkValidity()) {
                e.preventDefault();
            }

            eform.classList.add("was-validated");
        });

        this.settings = Object.assign(this.settings, { form: eform, inputs: selectInputs });
    }
}