!function ($) {
    "use strict";

    var TagsInput = function (element, options) {
        this.options = $.extend({}, $.fn.tagsInput.defaults, options);

        this.$element = $(element);
        this.options.hide && this.$element.hide();

        var id = this.$element.attr("id") || this.$element.attr("id", "tags_" + new Date().valueOf()).attr("id");

        this.$holder = $('<div id="' + id + '_tagsinput" class="tagsinput"></div>').insertAfter(this.$element);
        this.$inputWrapper = $('<div id="' + id + '_addTag"></div>').appendTo(this.$holder);
        if (this.options.interactive)
            this.$fakeInput = $('<input id="' + id + '_tag" value="" />').appendTo(this.$inputWrapper);
        $('<div class="tags_clear"></div>').appendTo(this.$holder);

        this.$holder
            .css('width', this.options.width)
            .css('height', this.options.height);
        //.captureScroll();

        if (this.options.interactive) {
            this.$fakeInput
                .css('color', this.options.placeholderColor)
                .resetAutosize(this.options);

            this.$holder.bind('click', this, function (event) {
                event.data.$fakeInput.focus();
            });

            this.$fakeInput.bind('focus', this, function (event) {
                event.data.$fakeInput.css('color', '#000');
            });

            this.$fakeInput.bind('input propertychange', this, function (event) {
                var val = event.data.$fakeInput.val().replace(/\s+/g, '');
                if (event.data.$fakeInput.val() != val) {
                    event.data.$fakeInput.val(val);
                    event.data.$fakeInput.doAutosize(event.data.options);
                }
            });

            if (this.options.autocomplete) {
                if ($.ui.autocomplete) {
                    this.$fakeInput.autocomplete(this.options.autocomplete);
                }
            } else {
                this.$fakeInput.bind('blur', this, function (event) {
                    var inputval = $(this).val();
                    if (inputval != '' && inputval != $(this).attr('placeholder')) {
                        if ((event.data.options.minChars <= inputval.length) && (!event.data.options.maxChars || (event.data.options.maxChars >= inputval.length)))
                            event.data.addTag(inputval, { focus: true });
                    } else {
                        $(this).css('color', event.data.options.placeholderColor);
                    }
                    return false;
                });
            }
            // if user types a comma, create a new tag
            this.$fakeInput.bind('keypress', this, function (event) {
                if (event.which == event.data.options.delimiter.charCodeAt(0) || event.which == 13) {
                    event.preventDefault();
                    if (event.data.$fakeInput.hasClass("not_valid"))
                        return false;
                    var inputval = $(this).val();
                    if ((event.data.options.minChars <= inputval.length) && (!event.data.options.maxChars || (event.data.options.maxChars >= inputval.length)))
                        event.data.addTag(inputval, { focus: true });
                    //$(this).resetAutosize(event.data.options);
                    return false;
                }
                $(this).doAutosize(event.data.options);
            });
            //Delete last tag on backspace
            this.options.removeWithBackspace && this.$fakeInput.bind('keydown', this, function (event) {
                if (event.keyCode == 8 && $(this).val() == '') {
                    event.preventDefault();
                    var last_tag = $(this).closest('.tagsinput').find('.tag:last');
                    if (last_tag.length) event.data.removeTag({ text: $("span", last_tag).text(), value: last_tag.data().val, $element: last_tag });
                    $(this).trigger('focus');
                }
            });
            this.$fakeInput.blur();
            //Removes the not_valid class when user changes the value of the fake input
            this.options.unique && this.$fakeInput.keydown(function (event) {
                if (event.keyCode == 8 || String.fromCharCode(event.which).match(/\w+|[áéíóúÁÉÍÓÚñÑ,/]+/)) {
                    $(this).removeClass('not_valid');
                }
            });
        } // if settings.interactive
    };

    TagsInput.prototype = {
        constructor: TagsInput,
        addTag: function (tag, options) {
            this.options = $.extend(this.options, { focus: false, callback: true }, options);

            var tagslist = this.$element.val().split(this.options.delimiter);
            if (tagslist[0] == '')
                tagslist = new Array();

            var tag = this.options.formatTag(tag);

            if (!tag || !tag.value.length || (this.options.unique && this.tagExist(tag))) {
                this.$fakeInput.addClass('not_valid');
                return;
            }
            this.$fakeInput.removeClass('not_valid');

            tag.$element = $('<span>').addClass('tag').attr("data-val", tag.value)
                .append(
                $('<span>').text(tag.text),
                $('<a>', {
                    href: 'javascript:;',
                    title: '移除',
                    text: '×'
                }).addClass("a-visibility").click({ tagsInput: this, tag: tag }, function (event) {
                    event.data.tagsInput.removeTag(event.data.tag);
                })
            ).insertBefore(this.$inputWrapper);

            tagslist.push(tag.value);

            this.$fakeInput.val('');
            if (this.options.focus) {
                this.$fakeInput.focus();
            } else {
                this.$fakeInput.blur();
            }
            this.$element.val(tagslist.join(this.options.delimiter));

            this.options.onAddTag && this.options.onAddTag(this, tag);
        },
        removeTag: function (tag) {
            tag.$element.remove();
            var tags = $.map($(".tag", this.$holder), function (el) {
                var dataval = $(el).data().val;
                if (dataval == tag.value)
                    $(el).remove();
                else
                    return dataval;
            });
            this.$element.val(tags.join(this.options.delimiter));

            if (this.options.onRemoveTag)
                this.options.onRemoveTag(this, tag);
        },
        tagExist: function (tags) {
            var tagslist = this.$element.val().split(this.options.delimiter);
            return $.inArray(tags.value, tagslist) > -1; //true when tag exists, false when not
        },
        importTags: function (tags) {
            var that = this;
            this.$element.val("");
            $(".tag", this.$holder).remove();
            $(".tag", this.$holder).each(function (el) {
                var tag = { text: $("span", el).text(), value: $(el).data().val, $element: this };
                if (that.options.onRemoveTag)
                    that.options.onRemoveTag(that, tag);
            });
            if (tags && tags.length) {
                for (i = 0; i < tags.length; i++) {
                    var el = this.addTag(tags[i], { focus: false, callback: false });
                    if (this.options.onAddTag)
                        this.options.onAddTag(this, tags[i], el);
                }
            }
        }
    };

    $.fn.tagsInput = function (option) {
        var $arguments = arguments;
        return this.each(function () {
            var $this = $(this)
              , data = $this.data('tagsinput')
              , options = $.extend({}, $.fn.tagsInput.defaults, $this.data(), typeof option == 'object' && option);
            if (!data) $this.data('tagsinput', (data = new TagsInput(this, options)))
            if (typeof option == 'string') data[option].apply(data, $.grep($arguments, function (n, i) { return i > 0; }));
        })
    };

    $.fn.doAutosize = function (o) {
        var minWidth = $(this).data('minwidth'),
	        maxWidth = $(this).data('maxwidth'),
	        val = '',
	        input = $(this),
	        testSubject = $('#' + $(this).data('tester_id'));

        if (val === (val = input.val())) { return; }

        // Enter new content into testSubject
        var escaped = val.replace(/&/g, '&amp;').replace(/\s/g, ' ').replace(/</g, '&lt;').replace(/>/g, '&gt;');
        testSubject.html(escaped);
        // Calculate new width + whether to change
        var testerWidth = testSubject.width(),
	        newWidth = (testerWidth + o.comfortZone) >= minWidth ? testerWidth + o.comfortZone : minWidth,
	        currentWidth = input.width(),
	        isValidWidthChange = (newWidth < currentWidth && newWidth >= minWidth)
	                             || (newWidth > minWidth && newWidth < maxWidth);

        // Animate width
        if (isValidWidthChange) {
            input.width(newWidth);
        }
    };

    $.fn.resetAutosize = function (options) {
        // alert(JSON.stringify(options));
        var minWidth = $(this).data('minwidth') || options.minInputWidth || $(this).width(),
            maxWidth = $(this).data('maxwidth') || options.maxInputWidth || ($(this).closest('.tagsinput').width() - options.inputPadding),
            val = '',
            input = $(this),
            testSubject = $('<tester/>').css({
                position: 'absolute',
                top: -9999,
                left: -9999,
                width: 'auto',
                fontSize: input.css('fontSize'),
                fontFamily: input.css('fontFamily'),
                fontWeight: input.css('fontWeight'),
                letterSpacing: input.css('letterSpacing'),
                whiteSpace: 'nowrap'
            }),
            testerId = $(this).attr('id') + '_autosize_tester';
        if (!$('#' + testerId).length > 0) {
            testSubject.attr('id', testerId);
            testSubject.appendTo('body');
        }

        input.data('minwidth', minWidth);
        input.data('maxwidth', maxWidth);
        input.data('tester_id', testerId);
        input.css('width', minWidth);
    };

    $.fn.tagsInput.defaults = {
        interactive: true,
        defaultText: '添加',
        minChars: 0,
        maxChars: 255,
        width: '300px',
        height: '100px',
        autocomplete: {},
        'hide': true,
        'delimiter': ',',
        'unique': true,
        removeWithBackspace: true,
        placeholderColor: '#666666',
        autosize: true,
        comfortZone: 20,
        inputPadding: 6 * 2,
        formatTag: function (tag) {
            return {
                text: (tag.text || tag).toString().replace(/\s/g, ''),
                value: (tag.value || tag.text || tag).toString().replace(/\s/g, '')
            }
        }
    };

    $.fn.tagsInput.Constructor = TagsInput;

}(window.jQuery);