var JsHelpers = function () {
    return {
        Util: function () {
            function newGuid() {
                var d = new Date().getTime();
                var uuid = 'xxxxxxxx-xxxx-4xxx-yxxx-xxxxxxxxxxxx'.replace(/[xy]/g, function (c) {
                    var r = (d + Math.random() * 16) % 16 | 0;
                    d = Math.floor(d / 16);
                    return (c == 'x' ? r : (r & 0x7 | 0x8)).toString(16);
                });
                return uuid;
            };
            return {
                SerializeObject: function (idDiv) {
                    var o = {};
                    var a = $('#' + idDiv + ' :input').serializeArray();
                    $.each(a, function () {
                        if (o[this.name]) {
                            if (!o[this.name].push) {
                                o[this.name] = [o[this.name]];
                            }
                            o[this.name].push(this.value || '');
                        } else {
                            o[this.name] = this.value || '';
                        }
                    });
                    return o;
                },
                Extend: function (parms) {
                    if (parms.length == 2) {
                        if (parms[0] == undefined && parms[1] == undefined) {
                            return {};
                        } else {
                            if (parms[0] == undefined || parms[1] == undefined) {
                                if (parms[0] == undefined) {
                                    return parms[1];
                                }

                                if (parms[0] == undefined) {
                                    return parms[0];
                                }
                            }
                        }

                        return $.extend({}, parms[0], parms[1]);
                    }

                    if (parms.length > 2) {
                        var toReturn = $.extend({}, parms[0], parms[1]);
                        for (var x = 2; x < parms.length; x++) {
                            toReturn = $.extend({}, toReturn, parms[x]);
                        }

                        return toReturn;
                    }

                    return {};
                },
                Guid: function () {
                    return newGuid().replace("-", "");
                },
                IsNullOrEmpty: function (value) {
                    return value == undefined || value == null || value == "" || value == "null" || jQuery.trim(value) == "";
                },
                IfIsNullOrEmpty: function (value, otherValue) {
                    if (JsHelpers.Util.IsNullOrEmpty(value)) {
                        return otherValue;
                    }

                    return value;
                },
                Timeout: function (func, time) {
                    if (time == undefined) {
                        time = 100;
                    }

                    setTimeout(func, time);
                }
            };
        }(),

        Ajax: function () {
            function executeAjax(type, parms) {

                if (parms.url == undefined) {
                    JsHelpers.Modal.Alert("O parâmetro 'parms.url' não pode ser nulo.");
                    return false;
                }

                var parameters = {
                    showLoader: parms.showLoader == undefined ? true : parms.showLoader,
                    contentType: parms.contentType == undefined ? 'application/json;charset=utf-8' : parms.contentType,
                    async: parms.async == undefined ? true : parms.async,
                    data: parms.data == undefined ? {} : parms.data,
                    cache: parms.cache == undefined ? false : parms.ajax,
                    timeout: parms.timeout == undefined ? 21000 : parms.timeout
                };

                if (parameters.showLoader)
                    JsHelpers.Wait.Open();

                var returnValue = null;
                $.ajax({
                    type: type,
                    url: parms.url,
                    data: parameters.data,
                    async: parameters.async,
                    cache: parameters.cache,
                    timeout: parameters.timeout,
                    contentType: parameters.contentType,
                    beforeSend: function () {
                        if ($.isFunction(parms.beforeSend))
                            $.when(parms.beforeSend())
                    },
                    success: function (data) {
                        if ($.isFunction(parms.success)) {
                            try {
                                parms.success(data);
                            } catch (e) {
                                JsHelpers.Alert.Error("Erro ao processar retorno", e.message);
                            }
                        }
                        else
                            returnValue = data;
                    },
                    error: function (err, timeout) {
                        try {
                            if ($.isFunction(parms.error)) {
                                parms.error(err.responseJSON.error);
                            } else if (timeout == 'timeout') {
                                JsHelpers.Alert.Warning("", "A sua requisição foi interrompida (time out).");
                            } else {
                                var erroSistema = err.responseJSON.error;
                                var detalhes = erroSistema.message;

                                if (erroSistema.code == 406) {
                                    JsHelpers.Alert.Warning("", detalhes);
                                } else {
                                    JsHelpers.Alert.Error("", detalhes);
                                }
                            }
                        } catch (e) {
                            if (err.statusText == 'timeout') {
                                JsHelpers.Alert.Warning("", "A sua requisição foi interrompida (time out).");
                            } else {
                                JsHelpers.Alert.Error("", err.statusText);
                            }
                        }
                    },
                    complete: function () {
                        if ($.isFunction(parms.complete)) {
                            $.when(parms.complete()).done(function () {
                                if (parameters.showLoader)
                                    JsHelpers.Wait.Close();
                            });
                        } else {
                            if (parameters.showLoader)
                                JsHelpers.Wait.Close();
                        }
                    }
                });

                return returnValue == null ? false : returnValue;
            }
            return {
                Get: function (parms) {
                    return executeAjax("GET", parms);
                },
                Post: function (parms) {
                    return executeAjax("POST", parms);
                },
                Delete: function (parms) {
                    return executeAjax("DELETE", parms);
                },
                Put: function (parms) {
                    return executeAjax("PUT", parms);
                }
            };
        }(),

        Alert: function () {
            function message(title, body, type, icon) {
                $.notify(
                    {
                        title: "<strong>" + title + "</strong> ",
                        message: body
                        //   icon: "fa " + icon + " fadeInRight animated"
                    },
                    {
                        element: 'body',
                        position: null,
                        allow_dismiss: true,
                        newest_on_top: false,
                        showProgressbar: false,
                        placement: {
                            from: "top",
                            align: "right"
                        },
                        type: type,
                        animate: {
                            enter: 'animated fadeInRight',
                            exit: 'animated fadeOutRight'
                        }
                    }
                );
            }
            return {
                Retorno: function (objRetorno) {
                    sbTitle = objRetorno.prpTitulo;
                    sbContent = objRetorno.prpMensagens[0];
                    if (objRetorno.prpPonto)
                        sbContent += " (" + objRetorno.prpPonto + ")";

                    switch (objRetorno.prpTipoMensagem) {
                        case 1:
                            JsHelpers.Alert.Success(sbTitle, sbContent);
                            break;
                        case 2:
                            JsHelpers.Alert.Warning(sbTitle, sbContent);
                            break;
                        case 3:
                            JsHelpers.Alert.Error(sbTitle, sbContent);
                            break;
                        case 4:
                            JsHelpers.Alert.Information(sbTitle, sbContent);
                            break;
                        default:
                            break;
                    }
                },
                Success: function (body, title) {
                    sbTitle = JsHelpers.Util.IsNullOrEmpty(title) ? "Concluído" : title;
                    sbContent = JsHelpers.Util.IsNullOrEmpty(body) ? "Processamento concluído com sucesso." : body;

                    return message(sbTitle, sbContent, "success", "fa-check");
                },
                Information: function (body, title) {
                    sbTitle = JsHelpers.Util.IsNullOrEmpty(title) ? "Informação" : title;
                    sbContent = JsHelpers.Util.IsNullOrEmpty(body) ? "..." : body;

                    return message(sbTitle, sbContent, "info", "fa-info");
                },
                Error: function (body, title) {
                    sbTitle = JsHelpers.Util.IsNullOrEmpty(title) ? "Erro" : title;
                    sbContent = JsHelpers.Util.IsNullOrEmpty(body) ? "Ocorreu algum erro durante o processamento." : body;

                    return message(sbTitle, sbContent, "danger", "fa-times");
                },
                Warning: function (body, title) {
                    sbTitle = JsHelpers.Util.IsNullOrEmpty(title) ? "Aviso" : title;
                    sbContent = JsHelpers.Util.IsNullOrEmpty(body) ? "Parametrôs inválidos para concluir o processo." : body;

                    return message(sbTitle, sbContent, "warning", "fa-exclamation-triangle");
                }
            }
        }(),

        Wait: function () {
            var pleaseWaitDiv = $(
                '   <div id="loader-wrapper">'
                + '     <div id="loader"></div>'
                + '     <div class="loader-section section-left"></div>'
                + '     <div class="loader-section section-right"></div>'
                + ' </div>');
            return {
                Open: function () {
                    $('#page-top').removeClass('loaded');
                    if ($("#loader-wrapper").length == 0)
                        $('#page-top').append(pleaseWaitDiv);
                },
                Close: function (text) {
                    $('#page-top').addClass('loaded');
                }
            };
        }(),

        Modal: function () {
            return {
                Dynamic: function (html, onShow, onHidden) {
                    $(html).modal().on('hidden.bs.modal', function () {
                        if (jQuery.isFunction(onHidden)) {
                            onHidden();
                        }

                        $(html).empty().remove();

                        JsHelpers.Util.Timeout(function () {
                            JsHelpers.Wait.Close();
                        }, 300);
                    }).on('show.bs.modal', function (e) {
                        var widthValue = parseInt($(window).width() / 100 * 80);
                        var width = widthValue + 'px';

                        $(this).find('.modal-dialog').css({
                            'width': width,
                            'max-width': width,
                            'min-width': width,
                        });

                        $('.date').datepicker({
                            format: "dd/mm/yyyy",
                            language: "pt-BR",
                            autoclose: true,
                            todayHighlight: true
                        });

                        if (jQuery.isFunction(onShow)) {
                            onShow();
                        }

                        JsHelpers.Util.Timeout(function () {
                            JsHelpers.Wait.Close();
                        }, 300);

                    }).modal('show');
                },
                Alert: function (texto, parms) {
                    var guid = JsHelpers.Util.Guid();
                    if (parms == undefined) {
                        parms = {};
                    }
                    //var modal = $('<div class="modal" id="' + guid + '" data-backdrop="static" data-keyboard="false"><div class="modal-header"><h3>' + texto + '</h3></div><div class="modal-body"><div class="progress progress-striped active"><div class="bar" style="width: 100%;"></div></div></div></div>');
                    var title = parms.Titulo != undefined ? parms.Titulo : 'Atenção:';

                    var modal = [
                        '<div class="modal fade" id="' + guid + '" data-backdrop="static" data-keyboard="false" aria-hidden="true">',
                        '   <div class="modal-dialog">',
                        '       <div class="modal-content">',
                        '           <div class="modal-header">',
                        '               <h3>' + title + '</h3>',
                        '            </div>',
                        '            <div class="modal-body">',
                        '                <div class="row text-center">',
                        '                   ' + texto,
                        '               </div>',
                        '            </div>',
                        '            <div class="modal-footer">',
                        //'                <button type="button" class="btn-sim btn btn-default" data-dismiss="modal">Sim</button>',
                        '               <button type="button" class="btn-nao btn btn-primary" data-dismiss="modal">Fechar</button>',
                        '           </div>',
                        '       </div>',
                        '   </div>',
                        '</div>'
                    ].join(' ');


                    $(modal).modal({ show: false }).on('show.bs.modal', function () {
                        //var widthValue = parseInt($(window).width() / 100 * 80);
                        //var width = widthValue + 'px';

                        $(this).find('.modal-dialog').css({
                            'top': '48%',
                            //'max-width': width,
                            //'min-width': width,
                        });

                        if (jQuery.isFunction(parms.onShow)) {
                            parms.onShow();
                        }

                        JsHelpers.Wait.Close();
                    }).on('hidden.bs.modal', function () {
                        if (jQuery.isFunction(parms.onHidden)) {
                            parms.onHidden();
                        }

                        $("#" + guid).empty().remove();
                        JsHelpers.Wait.Close();
                    }).modal('show');
                },
                Confirm: function (content, onConfirm, onCancel, onHidden, onShow) {
                    var guid = JsHelpers.Util.Guid();

                    var template = [
                        '<div class="modal" id="' + guid + '" data-backdrop="static" data-keyboard="false">',
                        '   <div class="modal-dialog">',
                        '       <div class="modal-content">',
                        '           <div class="modal-header">',
                        '               <h3>Confirmação:</h3>',
                        '            </div>',
                        '            <div class="modal-body">',
                        '                <div class="row text-center">',
                        '                   ' + content,
                        '               </div>',
                        '            </div>',
                        '            <div class="modal-footer">',
                        '               <button type="button" class="btn-nao btn btn-primary" data-dismiss="modal">Não</button>',
                        '               <button type="button" class="btn-sim btn btn-primary" data-dismiss="modal">Sim</button>',
                        '           </div>',
                        '       </div>',
                        '   </div>',
                        '</div>'
                    ].join(' ');

                    var modal = $(template).modal({ show: false });
                    $(modal).on('show.bs.modal', function () {
                        var dialog = $(this).find('.modal-dialog');
                        if (jQuery.isFunction(onConfirm)) {
                            $(dialog).find('.btn-sim').on('click', function () {
                                onConfirm();
                            });
                        }

                        if (jQuery.isFunction(onCancel)) {
                            $(dialog).find('.btn-nao').on('click', function () {
                                onCancel();
                            });
                        }

                        if (jQuery.isFunction(onShow)) {
                            onShow();
                        }

                        JsHelpers.Wait.Close();
                    });

                    $(modal).on('hidden.bs.modal', function () {
                        if (jQuery.isFunction(onHidden)) {
                            onHidden();
                        }

                        $("#" + guid).empty().remove();
                        JsHelpers.Wait.Close();
                    });

                    $(modal).modal('show');
                }
            };
        }()
    };
}();