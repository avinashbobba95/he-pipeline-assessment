var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
import { Component, h, Prop, State } from '@stencil/core';
import TrashCanIcon from '../../icons/trash-can';
import PlusIcon from '../../icons/plus_icon';
import { 
//IntellisenseContext,
SyntaxNames } from '../../models/elsa-interfaces';
import { SingleChoiceActivity } from '../../models/custom-component-models';
import { IconProvider, } from '../icon-provider/icon-provider';
function parseJson(json) {
    if (!json)
        return null;
    try {
        return JSON.parse(json);
    }
    catch (e) {
        console.warn(`Error parsing JSON: ${e}`);
    }
    return undefined;
}
let ElsaSingleChoiceRecordsProperty = class ElsaSingleChoiceRecordsProperty {
    constructor() {
        this.singleChoiceModel = new SingleChoiceActivity();
        this.iconProvider = new IconProvider();
        this.supportedSyntaxes = [SyntaxNames.JavaScript, SyntaxNames.Liquid];
        this.syntaxMultiChoiceCount = 0;
    }
    async componentWillLoad() {
        const propertyModel = this.propertyModel;
        const choicesJson = propertyModel.expressions[SyntaxNames.Json];
        this.singleChoiceModel = parseJson(choicesJson) || this.defaultActivityModel();
    }
    defaultActivityModel() {
        var activity = new SingleChoiceActivity();
        activity.choices = [];
        return activity;
    }
    updatePropertyModel() {
        this.propertyModel.expressions[SyntaxNames.Json] = JSON.stringify(this.singleChoiceModel);
    }
    onAddChoiceClick() {
        const choiceName = `Choice ${this.singleChoiceModel.choices.length + 1}`;
        const newChoice = { answer: choiceName };
        this.singleChoiceModel = Object.assign(Object.assign({}, this.singleChoiceModel), { choices: [...this.singleChoiceModel.choices, newChoice] });
        this.updatePropertyModel();
    }
    onDeleteChoiceClick(singleChoice) {
        this.singleChoiceModel = Object.assign(Object.assign({}, this.singleChoiceModel), { choices: this.singleChoiceModel.choices.filter(x => x != singleChoice) });
        this.updatePropertyModel();
    }
    onChoiceNameChanged(e, singleChoice) {
        singleChoice.answer = e.currentTarget.value.trim();
        this.updatePropertyModel();
    }
    render() {
        const choices = this.singleChoiceModel.choices;
        const renderChoiceEditor = (singleChoice, index) => {
            return (h("tr", { key: `choice-${index}` },
                h("td", { class: "elsa-py-2 elsa-pr-5" },
                    h("input", { type: "text", value: singleChoice.answer, onChange: e => this.onChoiceNameChanged(e, singleChoice), class: "focus:elsa-ring-blue-500 focus:elsa-border-blue-500 elsa-block elsa-w-full elsa-min-w-0 elsa-rounded-md sm:elsa-text-sm elsa-border-gray-300" })),
                h("td", { class: "elsa-pt-1 elsa-pr-2 elsa-text-right" },
                    h("button", { type: "button", onClick: () => this.onDeleteChoiceClick(singleChoice), class: "elsa-h-5 elsa-w-5 elsa-mx-auto elsa-outline-none focus:elsa-outline-none" },
                        h(TrashCanIcon, { options: this.iconProvider.getOptions() })))));
        };
        return (h("div", null,
            h("table", { class: "elsa-min-w-full elsa-divide-y elsa-divide-gray-200" },
                h("thead", { class: "elsa-bg-gray-50" },
                    h("tr", null,
                        h("th", { class: "elsa-py-3 elsa-text-left elsa-text-xs elsa-font-medium elsa-text-gray-500 elsa-tracking-wider elsa-w-10/12" }, "Answer"),
                        h("th", { class: "elsa-py-3 elsa-text-left elsa-text-xs elsa-font-medium elsa-text-gray-500 elsa-tracking-wider elsa-w-1/12" }, "\u00A0"))),
                h("tbody", null, choices.map(renderChoiceEditor))),
            h("button", { type: "button", onClick: () => this.onAddChoiceClick(), class: "elsa-inline-flex elsa-items-center elsa-px-4 elsa-py-2 elsa-border elsa-border-transparent elsa-shadow-sm elsa-text-sm elsa-font-medium elsa-rounded-md elsa-text-white elsa-bg-blue-600 hover:elsa-bg-blue-700 focus:elsa-outline-none focus:elsa-ring-2 focus:elsa-ring-offset-2 focus:elsa-ring-blue-500 elsa-mt-2" },
                h(PlusIcon, { options: this.iconProvider.getOptions() }),
                "Add Choice")));
    }
};
__decorate([
    Prop()
], ElsaSingleChoiceRecordsProperty.prototype, "activityModel", void 0);
__decorate([
    Prop()
], ElsaSingleChoiceRecordsProperty.prototype, "propertyDescriptor", void 0);
__decorate([
    Prop()
], ElsaSingleChoiceRecordsProperty.prototype, "propertyModel", void 0);
__decorate([
    State()
], ElsaSingleChoiceRecordsProperty.prototype, "singleChoiceModel", void 0);
__decorate([
    State()
], ElsaSingleChoiceRecordsProperty.prototype, "iconProvider", void 0);
ElsaSingleChoiceRecordsProperty = __decorate([
    Component({
        tag: 'elsa-singlechoice-records-property',
        shadow: false,
    })
], ElsaSingleChoiceRecordsProperty);
export { ElsaSingleChoiceRecordsProperty };
