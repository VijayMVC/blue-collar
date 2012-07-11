﻿/**
 * Manages the root history view.
 *
 * @constructor
 * @extends {AreaView}
 */
var HistoryView = AreaView.extend({
    template: _.template($('#history-template').html()),

    /**
     * Initialization.
     *
     * @param {Object} options Initialization options.
     */
    initialize: function(options) {
        AreaView.prototype.initialize.call(this, options);

        this.listView = new HistoryListView({model: this.model});
        this.listView.bind('display', this.display, this);
        this.listView.bind('signal', this.signal, this);
    },

    /**
     * Renders the ID view for the given model in the given details element.
     *
     * @param {jQuery} el The jQuery object containing the details element to render into.
     * @param {CollarModel} model The model to render the ID view for.
     */
    renderIdView: function(el, model) {
        var view = new HistoryDisplayView({model: model});
        view.bind('cancel', this.displayCancel, this);

        el.html(view.render().el);
        view.focus();

        if (!model.get('DetailsLoaded')) {
            this.trigger('details', this, {Model: model});
        }
    }
});