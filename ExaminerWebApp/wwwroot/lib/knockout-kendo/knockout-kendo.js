﻿/*
 * knockout-kendo 0.9.7
 * Copyright © 2015 Ryan Niemeyer & Telerik
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at

 * http://www.apache.org/licenses/LICENSE-2.0

 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */
!function (a) { "function" == typeof require && "object" == typeof exports && "object" == typeof module ? a(require("knockout"), require("jquery"), require("kendo")) : "function" == typeof define && define.amd ? define(["knockout", "jquery", "kendo"], a) : a(window.ko, window.jQuery, window.kendo) }(function (a, b, c, d) { c = c || window.kendo, a.kendo = a.kendo || {}; var e = a.utils.unwrapObservable; a.kendo.BindingFactory = function () { var f = this; this.createBinding = function (d) { if (b()[d.parent || d.name]) { var e = {}; e.init = function (a, b, c, g, h) { var i = f.buildOptions(d, b); return i.async === !0 || d.async === !0 && i.async !== !1 ? (setTimeout(function () { e.setup(a, i, h) }, 0), void 0) : (e.setup(a, i, h), i && i.useKOTemplates ? { controlsDescendantBindings: !0 } : void 0) }, e.setup = function (e, g, h) { var i, j = b(e); f.setupTemplates(d.templates, g, e, h), i = f.getWidget(d, g, j), f.handleEvents(g, d, e, i, h), f.watchValues(i, g, d, e), i.destroy && a.utils.domNodeDisposal.addDisposeCallback(e, function () { i.element && ("function" == typeof c.destroy ? c.destroy(i.element) : i.destroy()) }) }, e.options = {}, e.widgetConfig = d, a.bindingHandlers[d.bindingName || d.name] = e } }, this.buildOptions = function (b, d) { var f = b.defaultOption, g = a.utils.extend({}, a.bindingHandlers[b.name].options), h = e(d()); return h instanceof c.data.DataSource || "object" != typeof h || null === h || f && !(f in h) ? g[f] = d() : a.utils.extend(g, h), g }; var g = function (b, c) { return function (d) { return a.renderTemplate(b, c.createChildContext(d._raw && d._raw() || d)) } }; this.setupTemplates = function (b, c, d, e) { var f, h, i, j; if (b && c && c.useKOTemplates) { for (f = 0, h = b.length; h > f; f++) i = b[f], c[i] && (c[i] = g(c[i], e)); j = c.dataBound, c.dataBound = function () { a.memoization.unmemoizeDomNodeAndDescendants(d), j && j.apply(this, arguments) } } }, this.unwrapOneLevel = function (a) { var b, d = {}; if (a) if (a instanceof c.data.DataSource) d = a; else if ("object" == typeof a) for (b in a) d[b] = e(a[b]); return d }, this.getWidget = function (b, c, d) { var e; if (b.parent) { var f = d.closest("[data-bind*='" + b.parent + ":']"); e = f.length ? f.data(b.parent) : null } else e = d[b.name](this.unwrapOneLevel(c)).data(b.name); return a.isObservable(c.widget) && c.widget(e), e }, this.watchValues = function (a, b, c, d) { var e, g = c.watch; if (g) for (e in g) g.hasOwnProperty(e) && f.watchOneValue(e, a, b, c, d) }, this.watchOneValue = function (c, f, g, h, i) { var j = a.computed({ read: function () { var a, j, k = h.watch[c], l = e(g[c]), m = h.parent ? [i] : []; b.isArray(k) ? k = f[l ? k[0] : k[1]] : "string" == typeof k ? k = f[k] : j = !0, k && g[c] !== d && (j ? m.push(l, g) : (a = k.apply(f, m), m.push(l)), (j || a !== l) && k.apply(f, m)) }, disposeWhenNodeIsRemoved: i }).extend({ throttle: g.throttle || 0 === g.throttle ? g.throttle : 1 }); a.isObservable(g[c]) || j.dispose() }, this.handleEvents = function (a, b, c, d, e) { var g, h, i = b.events; if (i) for (g in i) i.hasOwnProperty(g) && (h = i[g], "string" == typeof h && (h = { value: h, writeTo: h }), f.handleOneEvent(g, h, a, c, d, b.childProp, e)) }, this.handleOneEvent = function (b, c, d, e, f, g, h) { var i = "function" == typeof c ? c : d[c.call]; "function" == typeof c ? i = i.bind(h.$data, d) : c.call && "function" == typeof d[c.call] ? i = d[c.call].bind(h.$data, h.$data) : c.writeTo && a.isWriteableObservable(d[c.writeTo]) && (i = function (a) { var b, f; g && a[g] && a[g] !== e || (b = c.value, f = "string" == typeof b && this[b] ? this[b](g && e) : b, d[c.writeTo](f)) }), i && f.bind(b, i) } }, a.kendo.bindingFactory = new a.kendo.BindingFactory, a.kendo.setDataSource = function (b, d, e) { var f, g; return d instanceof c.data.DataSource ? (b.setDataSource(d), void 0) : (e && e.useKOTemplates || (f = a.mapping && d && d.__ko_mapping__, g = d && f ? a.mapping.toJS(d) : a.toJS(d)), b.dataSource.data(g || d), void 0) }, function () { var a = c.data.ObservableArray.fn.wrap; c.data.ObservableArray.fn.wrap = function (b) { var c = a.apply(this, arguments); return c._raw = function () { return b }, c } }(); var f = function (b) { return function (c) { c && (a.utils.extend(this.options[b], c), this.redraw(), this.value(.001 + this.value())) } }, g = function (a, c) { a ? this.open("string" == typeof c.target ? b(e(c.target)) : c.target) : this.element.parent().is(":visible") && this.close() }, h = a.kendo.bindingFactory.createBinding.bind(a.kendo.bindingFactory), i = "center", j = "check", k = "clicked", l = "close", m = "collapse", n = "content", o = "data", p = "date", q = "disable", r = "enable", s = "expand", t = "expanded", u = "error", v = "filter", w = "info", x = "isOpen", y = "max", z = "min", A = "open", B = "palette", C = "readonly", D = "scrollTo", E = "search", F = "select", G = "selected", H = "size", I = "success", J = "title", K = "value", L = "values", M = "warning", N = "zoom"; h({ name: "kendoAutoComplete", events: { change: K, open: { writeTo: x, value: !0 }, close: { writeTo: x, value: !1 } }, watch: { enabled: r, search: [E, l], data: function (b) { a.kendo.setDataSource(this, b) }, value: K } }), h({ name: "kendoButton", defaultOption: k, events: { click: { call: k } }, watch: { enabled: r } }), h({ name: "kendoCalendar", defaultOption: K, events: { change: K }, watch: { max: y, min: z, value: K } }), h({ name: "kendoColorPicker", events: { change: K, open: { writeTo: x, value: !0 }, close: { writeTo: x, value: !1 } }, watch: { enabled: r, value: K, color: K, palette: B } }), h({ name: "kendoComboBox", events: { change: K, open: { writeTo: x, value: !0 }, close: { writeTo: x, value: !1 } }, watch: { enabled: r, isOpen: [A, l], data: function (b) { a.kendo.setDataSource(this, b) }, value: K } }), h({ name: "kendoDatePicker", defaultOption: K, events: { change: K, open: { writeTo: x, value: !0 }, close: { writeTo: x, value: !1 } }, watch: { enabled: r, max: y, min: z, value: K, isOpen: [A, l] } }), h({ name: "kendoDateTimePicker", defaultOption: K, events: { change: K, open: { writeTo: x, value: !0 }, close: { writeTo: x, value: !1 } }, watch: { enabled: r, max: y, min: z, value: K, isOpen: [A, l] } }), h({ name: "kendoDropDownList", events: { change: K, open: { writeTo: x, value: !0 }, close: { writeTo: x, value: !1 } }, watch: { enabled: r, isOpen: [A, l], data: function (b) { a.kendo.setDataSource(this, b), b.length && this.options.optionLabel && this.select() < 0 && this.select(0) }, value: K } }), h({ name: "kendoEditor", defaultOption: K, events: { change: K }, watch: { enabled: r, value: K } }), h({ name: "kendoGantt", defaultOption: o, watch: { data: function (b) { a.kendo.setDataSource(this, b) } } }), h({ name: "kendoGrid", defaultOption: o, watch: { data: function (b, c) { a.kendo.setDataSource(this, b, c) } }, templates: ["rowTemplate", "altRowTemplate"] }), h({ name: "kendoListView", defaultOption: o, watch: { data: function (b, c) { a.kendo.setDataSource(this, b, c) } }, templates: ["template"] }), h({ name: "kendoPager", defaultOption: o, watch: { data: function (b, c) { a.kendo.setDataSource(this, b, c) }, page: "page" }, templates: ["selectTemplate", "linkTemplate"] }), h({ name: "kendoMaskedTextBox", defaultOption: K, events: { change: K }, watch: { enabled: r, isReadOnly: C, value: K } }), h({ name: "kendoMap", events: { zoomEnd: function (b, c) { a.isWriteableObservable(b.zoom) && b.zoom(c.sender.zoom()) }, panEnd: function (b, c) { var d; a.isWriteableObservable(b.center) && (d = c.sender.center(), b.center([d.lat, d.lng])) } }, watch: { center: i, zoom: N } }), h({ name: "kendoMenu", async: !0 }), h({ name: "kendoMenuItem", parent: "kendoMenu", watch: { enabled: r, isOpen: [A, l] }, async: !0 }), h({ name: "kendoMobileActionSheet", events: { open: { writeTo: x, value: !0 }, close: { writeTo: x, value: !1 } }, watch: { isOpen: g }, async: !0 }), h({ name: "kendoMobileButton", defaultOption: k, events: { click: { call: k } }, watch: { enabled: r } }), h({ name: "kendoMobileButtonGroup", events: { select: function (b, c) { a.isWriteableObservable(b.selectedIndex) && b.selectedIndex(c.sender.current().index()) } }, watch: { enabled: r, selectedIndex: F } }), h({ name: "kendoMobileDrawer", events: { show: { writeTo: x, value: !0 }, hide: { writeTo: x, value: !1 } }, watch: { isOpen: function (a) { this[a ? "show" : "hide"]() } }, async: !0 }), h({ name: "kendoMobileListView", defaultOption: o, events: { click: { call: k } }, watch: { data: function (b, c) { a.kendo.setDataSource(this, b, c) } }, templates: ["template"] }), h({ name: "kendoMobileModalView", events: { open: { writeTo: x, value: !0 }, close: { writeTo: x, value: !1 } }, watch: { isOpen: g }, async: !0 }), h({ name: "kendoMobileNavBar", watch: { title: J } }), h({ name: "kendoMobilePopOver", events: { open: { writeTo: x, value: !0 }, close: { writeTo: x, value: !1 } }, watch: { isOpen: g }, async: !0 }), h({ name: "kendoMobileScroller", events: { pull: function (a, b) { var c = b.sender.pullHandled.bind(b.sender); "function" == typeof a.pulled && a.pulled.call(this, this, b, c) } }, watch: { enabled: [r, q] } }), h({ name: "kendoMobileScrollView", events: { change: function (b, c) { (c.page || 0 === c.page) && a.isWriteableObservable(b.currentIndex) && b.currentIndex(c.page) } }, watch: { currentIndex: D, data: function (b) { a.kendo.setDataSource(this, b) } } }), h({ name: "kendoMobileSwitch", events: { change: function (b, c) { a.isWriteableObservable(b.checked) && b.checked(c.checked) } }, watch: { enabled: r, checked: j } }), h({ name: "kendoMobileTabStrip", events: { select: function (b, c) { a.isWriteableObservable(b.selectedIndex) && b.selectedIndex(c.item.index()) } }, watch: { selectedIndex: function (a) { (a || 0 === a) && this.switchTo(a) } } }), h({ name: "kendoMultiSelect", events: { change: K, open: { writeTo: x, value: !0 }, close: { writeTo: x, value: !1 } }, watch: { enabled: r, search: [E, l], data: function (b) { a.kendo.setDataSource(this, b) }, value: function (a) { this.dataSource.filter({}), this.value(a) } } }); var O = function (a, b) { b || 0 === b ? this.show(b, a) : this.hide() }; h({ name: "kendoNotification", watch: { error: function (a) { O.call(this, u, a) }, info: function (a) { O.call(this, w, a) }, success: function (a) { O.call(this, I, a) }, warning: function (a) { O.call(this, M, a) } } }), h({ name: "kendoNumericTextBox", defaultOption: K, events: { change: K, spin: K }, watch: { enabled: r, value: K, max: function (a) { this.options.max = a; var b = this.value(); (b || 0 === b) && b > a && this.value(a) }, min: function (a) { this.options.min = a; var b = this.value(); (b || 0 === b) && a > b && this.value(a) } } }), h({ name: "kendoPanelBar", async: !0 }), h({ name: "kendoPanelItem", parent: "kendoPanelBar", watch: { enabled: r, expanded: [s, m], selected: [F] }, childProp: "item", events: { expand: { writeTo: t, value: !0 }, collapse: { writeTo: t, value: !1 }, select: { writeTo: G, value: K } }, async: !0 }), h({ name: "kendoPivotGrid", watch: { data: function (b) { a.kendo.setDataSource(this, b) } } }), h({ name: "kendoProgressBar", defaultOption: K, events: { change: K }, watch: { enabled: r, value: K } }), h({ name: "kendoRangeSlider", defaultOption: L, events: { change: L }, watch: { values: L, enabled: r } }); var P = function (b) { return function (c, d) { var f = e(c.data || c.dataSource), g = e(c.idField) || "id", h = a.utils.arrayFirst(f, function (a) { return e(a[g]) === d.event[g] }), i = function (b) { for (var c in h) if (b.hasOwnProperty(c) && h.hasOwnProperty(c)) { var d = b[c], e = h[c]; a.isWriteableObservable(e) && e(d) } }; h && b(c, d, h, i) } }; h({ name: "kendoScheduler", events: { moveEnd: P(function (a, b, c, d) { d(b), d(b.resources) }), save: P(function (a, b, c, d) { d(b.event) }), remove: function (b, c) { var d, e = b.data || b.dataSource, f = a.unwrap(e); f && f.length && (d = a.utils.arrayFirst(a.unwrap(e), function (a) { return a.uuid === c.event.uuid }), d && (a.utils.arrayRemoveItem(f, d), a.isWriteableObservable(e) && e.valueHasMutated())) } }, watch: { data: function (b, c) { a.kendo.setDataSource(this, b, c) }, date: p }, async: !0 }), h({ name: "kendoSlider", defaultOption: K, events: { change: K }, watch: { value: K, enabled: r } }), h({ name: "kendoSortable", defaultOption: o, events: { end: function (b, c) { var d = "__ko_kendo_sortable_data__", e = "receive" !== c.action ? a.dataFor(c.item[0]) : c.draggableEvent[d], f = b.data, g = b.data; ("sort" === c.action || "remove" === c.action) && (g.splice(c.oldIndex, 1), "remove" === c.action && (c.draggableEvent[d] = e)), ("sort" === c.action || "receive" === c.action) && (g.splice(c.newIndex, 0, e), delete c.draggableEvent[d], c.sender.placeholder.remove()), f.valueHasMutated() } } }), h({ name: "kendoSplitter", async: !0 }), h({ name: "kendoSplitterPane", parent: "kendoSplitter", watch: { max: y, min: z, size: H, expanded: [s, m] }, childProp: "pane", events: { collapse: { writeTo: t, value: !1 }, expand: { writeTo: t, value: !0 }, resize: H }, async: !0 }), h({ name: "kendoTabStrip", async: !0 }), h({ name: "kendoTab", parent: "kendoTabStrip", watch: { enabled: r }, childProp: "item", async: !0 }), h({ name: "kendoToolBar" }), h({ name: "kendoTooltip", events: {}, watch: { content: function (a) { this.options.content = a, this.refresh() }, filter: v } }), h({ name: "kendoTimePicker", defaultOption: K, events: { change: K }, watch: { max: y, min: z, value: K, enabled: r, isOpen: [A, l] } }), h({ name: "kendoTreeMap", watch: { data: function (b) { a.kendo.setDataSource(this, b) } } }), h({ name: "kendoTreeView", watch: { data: function (b, c) { a.kendo.setDataSource(this, b, c) } }, events: { change: function (b, c) { if (a.isWriteableObservable(b.value)) { var d = c.sender; b.value(d.dataItem(d.select())) } } }, async: !0 }), h({ name: "kendoTreeItem", parent: "kendoTreeView", watch: { enabled: r, expanded: [s, m], selected: function (a, b) { b ? this.select(a) : this.select()[0] == a && this.select(null) } }, childProp: "node", events: { collapse: { writeTo: t, value: !1 }, expand: { writeTo: t, value: !0 }, select: { writeTo: G, value: !0 } }, async: !0 }), h({ name: "kendoUpload", watch: { enabled: r } }), h({ name: "kendoWindow", events: { open: { writeTo: x, value: !0 }, close: { writeTo: x, value: !1 } }, watch: { content: n, title: J, isOpen: [A, l] }, async: !0 }), h({ name: "kendoBarcode", watch: { value: K } }), h({ name: "kendoChart", watch: { data: function (b) { a.kendo.setDataSource(this, b) } } }), h({ name: "kendoLinearGauge", defaultOption: K, watch: { value: K, gaugeArea: f("gaugeArea"), pointer: f("pointer"), scale: f("scale") } }), h({ name: "kendoQRCode", watch: { value: K } }), h({ name: "kendoRadialGauge", defaultOption: K, watch: { value: K, gaugeArea: f("gaugeArea"), pointer: f("pointer"), scale: f("scale") } }), h({ name: "kendoSparkline", watch: { data: function (b) { a.kendo.setDataSource(this, b) } } }), h({ name: "kendoStockChart", watch: { data: function (b) { a.kendo.setDataSource(this, b) } } }) });