/* eslint-disable */
/* tslint:disable */
/**
 * This is an autogenerated file created by the Stencil compiler.
 * It contains typing information for all components that exist in this project.
 */
import { HTMLStencilElement, JSXBase } from "@stencil/core/internal";
import { ActivityDefinitionProperty, ActivityModel, ActivityPropertyDescriptor, IntellisenseContext } from "./models/elsa-interfaces";
import { CheckboxQuestion, IOutcomeProperty, IQuestionComponent, ITextProperty, RadioQuestion } from "./models/custom-component-models";
import { IconProvider } from "./components/icon-provider/icon-provider";
export namespace Components {
    interface ConditionalTextListProperty {
        "activityModel": ActivityModel;
        "propertyDescriptor": ActivityPropertyDescriptor;
        "propertyModel": ActivityDefinitionProperty;
    }
    interface CustomElsaSwitchCasesProperty {
        "activityModel": ActivityModel;
        "propertyDescriptor": ActivityPropertyDescriptor;
        "propertyModel": ActivityDefinitionProperty;
    }
    interface CustomInputProperty {
        "context"?: string;
        "customProperty": ITextProperty;
        "editorHeight": string;
        "index": number;
        "intellisenseContext": IntellisenseContext;
        "propertyDescriptor": ActivityPropertyDescriptor;
        "showLabel": boolean;
        "singleLineMode": boolean;
        "supportedSyntaxes": Array<string>;
    }
    interface CustomOutcomeProperty {
        "context"?: string;
        "editorHeight": string;
        "iconProvider": IconProvider;
        "index": number;
        "intellisenseContext": IntellisenseContext;
        "outcome": IOutcomeProperty;
        "propertyDescriptor": ActivityPropertyDescriptor;
        "showLabel": boolean;
        "singleLineMode": boolean;
        "supportedSyntaxes": Array<string>;
    }
    interface CustomTextProperty {
        "activityModel": ActivityModel;
        "editorHeight": string;
        "propertyDescriptor": ActivityPropertyDescriptor;
        "propertyModel": ActivityDefinitionProperty;
        "singleLine": boolean;
    }
    interface QuestionCheckboxProperty {
        "question": CheckboxQuestion;
    }
    interface QuestionProperty {
        "ActivityModel": ActivityModel;
        "question": IQuestionComponent;
    }
    interface QuestionPropertyV2 {
        "ActivityModel": ActivityModel;
        "Properties": Array<ActivityPropertyDescriptor>;
        "Type": string;
        "question": IQuestionComponent;
    }
    interface QuestionRadioProperty {
        "question": RadioQuestion;
    }
    interface QuestionScreenProperty {
        "activityModel": ActivityModel;
        "propertyDescriptor": ActivityPropertyDescriptor;
        "propertyModel": ActivityDefinitionProperty;
        "questionProperties": Array<ActivityPropertyDescriptor>;
    }
}
declare global {
    interface HTMLConditionalTextListPropertyElement extends Components.ConditionalTextListProperty, HTMLStencilElement {
    }
    var HTMLConditionalTextListPropertyElement: {
        prototype: HTMLConditionalTextListPropertyElement;
        new (): HTMLConditionalTextListPropertyElement;
    };
    interface HTMLCustomElsaSwitchCasesPropertyElement extends Components.CustomElsaSwitchCasesProperty, HTMLStencilElement {
    }
    var HTMLCustomElsaSwitchCasesPropertyElement: {
        prototype: HTMLCustomElsaSwitchCasesPropertyElement;
        new (): HTMLCustomElsaSwitchCasesPropertyElement;
    };
    interface HTMLCustomInputPropertyElement extends Components.CustomInputProperty, HTMLStencilElement {
    }
    var HTMLCustomInputPropertyElement: {
        prototype: HTMLCustomInputPropertyElement;
        new (): HTMLCustomInputPropertyElement;
    };
    interface HTMLCustomOutcomePropertyElement extends Components.CustomOutcomeProperty, HTMLStencilElement {
    }
    var HTMLCustomOutcomePropertyElement: {
        prototype: HTMLCustomOutcomePropertyElement;
        new (): HTMLCustomOutcomePropertyElement;
    };
    interface HTMLCustomTextPropertyElement extends Components.CustomTextProperty, HTMLStencilElement {
    }
    var HTMLCustomTextPropertyElement: {
        prototype: HTMLCustomTextPropertyElement;
        new (): HTMLCustomTextPropertyElement;
    };
    interface HTMLQuestionCheckboxPropertyElement extends Components.QuestionCheckboxProperty, HTMLStencilElement {
    }
    var HTMLQuestionCheckboxPropertyElement: {
        prototype: HTMLQuestionCheckboxPropertyElement;
        new (): HTMLQuestionCheckboxPropertyElement;
    };
    interface HTMLQuestionPropertyElement extends Components.QuestionProperty, HTMLStencilElement {
    }
    var HTMLQuestionPropertyElement: {
        prototype: HTMLQuestionPropertyElement;
        new (): HTMLQuestionPropertyElement;
    };
    interface HTMLQuestionPropertyV2Element extends Components.QuestionPropertyV2, HTMLStencilElement {
    }
    var HTMLQuestionPropertyV2Element: {
        prototype: HTMLQuestionPropertyV2Element;
        new (): HTMLQuestionPropertyV2Element;
    };
    interface HTMLQuestionRadioPropertyElement extends Components.QuestionRadioProperty, HTMLStencilElement {
    }
    var HTMLQuestionRadioPropertyElement: {
        prototype: HTMLQuestionRadioPropertyElement;
        new (): HTMLQuestionRadioPropertyElement;
    };
    interface HTMLQuestionScreenPropertyElement extends Components.QuestionScreenProperty, HTMLStencilElement {
    }
    var HTMLQuestionScreenPropertyElement: {
        prototype: HTMLQuestionScreenPropertyElement;
        new (): HTMLQuestionScreenPropertyElement;
    };
    interface HTMLElsaTextareaQuestionElement extends Components.ElsaTextareaQuestion, HTMLStencilElement {
    }
    var HTMLElsaTextareaQuestionElement: {
        prototype: HTMLElsaTextareaQuestionElement;
        new (): HTMLElsaTextareaQuestionElement;
    };
    interface HTMLElementTagNameMap {
        "conditional-text-list-property": HTMLConditionalTextListPropertyElement;
        "custom-elsa-switch-cases-property": HTMLCustomElsaSwitchCasesPropertyElement;
        "custom-input-property": HTMLCustomInputPropertyElement;
        "custom-outcome-property": HTMLCustomOutcomePropertyElement;
        "custom-text-property": HTMLCustomTextPropertyElement;
        "question-checkbox-property": HTMLQuestionCheckboxPropertyElement;
        "question-property": HTMLQuestionPropertyElement;
        "question-property-v2": HTMLQuestionPropertyV2Element;
        "question-radio-property": HTMLQuestionRadioPropertyElement;
        "question-screen-property": HTMLQuestionScreenPropertyElement;
    }
}
declare namespace LocalJSX {
    interface ConditionalTextListProperty {
        "activityModel"?: ActivityModel;
        "propertyDescriptor"?: ActivityPropertyDescriptor;
        "propertyModel"?: ActivityDefinitionProperty;
    }
    interface CustomElsaSwitchCasesProperty {
        "activityModel"?: ActivityModel;
        "propertyDescriptor"?: ActivityPropertyDescriptor;
        "propertyModel"?: ActivityDefinitionProperty;
    }
    interface CustomInputProperty {
        "context"?: string;
        "customProperty"?: ITextProperty;
        "editorHeight"?: string;
        "index"?: number;
        "intellisenseContext"?: IntellisenseContext;
        "onValueChanged"?: (event: CustomEvent<ITextProperty>) => void;
        "propertyDescriptor"?: ActivityPropertyDescriptor;
        "showLabel"?: boolean;
        "singleLineMode"?: boolean;
        "supportedSyntaxes"?: Array<string>;
    }
    interface CustomOutcomeProperty {
        "context"?: string;
        "editorHeight"?: string;
        "iconProvider"?: IconProvider;
        "index"?: number;
        "intellisenseContext"?: IntellisenseContext;
        "onDelete"?: (event: CustomEvent<IOutcomeProperty>) => void;
        "onValueChanged"?: (event: CustomEvent<IOutcomeProperty>) => void;
        "outcome"?: IOutcomeProperty;
        "propertyDescriptor"?: ActivityPropertyDescriptor;
        "showLabel"?: boolean;
        "singleLineMode"?: boolean;
        "supportedSyntaxes"?: Array<string>;
    }
    interface CustomTextProperty {
        "activityModel"?: ActivityModel;
        "editorHeight"?: string;
        "propertyDescriptor"?: ActivityPropertyDescriptor;
        "propertyModel"?: ActivityDefinitionProperty;
        "singleLine"?: boolean;
    }
    interface QuestionCheckboxProperty {
        "onUpdateQuestion"?: (event: CustomEvent<IQuestionComponent>) => void;
        "question"?: CheckboxQuestion;
    }
    interface QuestionProperty {
        "ActivityModel"?: ActivityModel;
        "onUpdateQuestion"?: (event: CustomEvent<IQuestionComponent>) => void;
        "question"?: IQuestionComponent;
    }
    interface QuestionPropertyV2 {
        "ActivityModel"?: ActivityModel;
        "Properties"?: Array<ActivityPropertyDescriptor>;
        "Type"?: string;
        "question"?: IQuestionComponent;
    }
    interface QuestionRadioProperty {
        "onUpdateQuestion"?: (event: CustomEvent<IQuestionComponent>) => void;
        "question"?: RadioQuestion;
    }
    interface QuestionScreenProperty {
        "activityModel"?: ActivityModel;
        "propertyDescriptor"?: ActivityPropertyDescriptor;
        "propertyModel"?: ActivityDefinitionProperty;
        "questionProperties"?: Array<ActivityPropertyDescriptor>;
    }
    interface ElsaTextareaQuestion {
        "onUpdateQuestion"?: (event: ElsaTextareaQuestionCustomEvent<IQuestionComponent>) => void;
        "question"?: TextAreaQuestion;
    }
    interface IntrinsicElements {
        "conditional-text-list-property": ConditionalTextListProperty;
        "custom-elsa-switch-cases-property": CustomElsaSwitchCasesProperty;
        "custom-input-property": CustomInputProperty;
        "custom-outcome-property": CustomOutcomeProperty;
        "custom-text-property": CustomTextProperty;
        "question-checkbox-property": QuestionCheckboxProperty;
        "question-property": QuestionProperty;
        "question-property-v2": QuestionPropertyV2;
        "question-radio-property": QuestionRadioProperty;
        "question-screen-property": QuestionScreenProperty;
    }
}
export { LocalJSX as JSX };
declare module "@stencil/core" {
    export namespace JSX {
        interface IntrinsicElements {
            "conditional-text-list-property": LocalJSX.ConditionalTextListProperty & JSXBase.HTMLAttributes<HTMLConditionalTextListPropertyElement>;
            "custom-elsa-switch-cases-property": LocalJSX.CustomElsaSwitchCasesProperty & JSXBase.HTMLAttributes<HTMLCustomElsaSwitchCasesPropertyElement>;
            "custom-input-property": LocalJSX.CustomInputProperty & JSXBase.HTMLAttributes<HTMLCustomInputPropertyElement>;
            "custom-outcome-property": LocalJSX.CustomOutcomeProperty & JSXBase.HTMLAttributes<HTMLCustomOutcomePropertyElement>;
            "custom-text-property": LocalJSX.CustomTextProperty & JSXBase.HTMLAttributes<HTMLCustomTextPropertyElement>;
            "question-checkbox-property": LocalJSX.QuestionCheckboxProperty & JSXBase.HTMLAttributes<HTMLQuestionCheckboxPropertyElement>;
            "question-property": LocalJSX.QuestionProperty & JSXBase.HTMLAttributes<HTMLQuestionPropertyElement>;
            "question-property-v2": LocalJSX.QuestionPropertyV2 & JSXBase.HTMLAttributes<HTMLQuestionPropertyV2Element>;
            "question-radio-property": LocalJSX.QuestionRadioProperty & JSXBase.HTMLAttributes<HTMLQuestionRadioPropertyElement>;
            "question-screen-property": LocalJSX.QuestionScreenProperty & JSXBase.HTMLAttributes<HTMLQuestionScreenPropertyElement>;
        }
    }
}
