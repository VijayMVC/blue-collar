﻿/**
 * Scheduled jobs area controller implementation.
 *
 * @constructor
 * @extends {CollarController}
 */
var ScheduledJobsController = CollarController.extend({
    collection: ScheduledJobCollection,
    fragment: 'schedules',

    /**
     * Initialization.
     *
     * @param {Object} options Initialization options.
     */
    initialize: function(options) {
        this.model.set({ScheduleName: ''}, {silent: true});
        this.view = new ScheduledJobsView({model: this.model});
        this.view.bind('fetch', this.fetch, this);
        this.view.bind('editDelete', this.editDelete, this);
        this.view.bind('editSubmit', this.editSubmit, this);
    },

    /**
     * Renders the index view.
     *
     * @param {Number} id The requested schedule ID.
     * @param {String} search The search string to filter the view on.
     * @param {Number} page The page number to filter the view on.
     * @param {Number} jid The requested record ID to display.
     * @param {String} action The requested record action to take.
     */
    index: function(id, search, page, jid, action) {
        if (id && !_.isNumber(id)) {
            id = parseInt(id, 10);

            if (id < 0 || isNaN(id)) {
                id = 0;
            }
        } else {
            id = 0;
        }

        if (page && !_.isNumber(page)) {
            page = parseInt(page, 10);
        } else {
            page = 1;
        }

        if (jid && !_.isNumber(jid)) {
            jid = parseInt(jid, 10);

            if (jid < 0 || isNaN(jid)) {
                jid = 0;
            }
        } else {
            jid = 0;
        }

        this.model.urlRoot = this.urlRoot + '/' + encodeURIComponent(id.toString()) + '/jobs';
        this.getCollection().urlRoot = this.model.urlRoot;
        
        this.model.set({ScheduleId: id, Search: search || '', PageNumber: page, Id: jid, Action: action || '', Loading: true}, {silent: true});
        this.view.delegateEvents();
        this.page.html(this.view.render().el);
        this.fetch();
    },

    /**
     * Gets the URL fragment to use when navigating.
     *
     * @return {String} A URL fragment.
     */
    navigateFragment: function() {
        var fragment = this.fragment,
            scheduleId = this.model.get('ScheduleId');

        if (scheduleId) {
            fragment += '/id/' + encodeURIComponent(scheduleId.toString()) + '/jobs';
        }

        return fragment;
    }
});