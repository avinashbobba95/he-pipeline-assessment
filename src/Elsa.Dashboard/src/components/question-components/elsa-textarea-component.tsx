import { Component, h, Prop, State, Event, EventEmitter } from '@stencil/core';

import {
  HTMLElsaMultiExpressionEditorElement,
  //IntellisenseContext,
  SyntaxNames
} from '../../models/elsa-interfaces';

import {
  IQuestionComponent, TextAreaQuestion
} from '../../models/custom-component-models';

import {
  IconProvider,
} from '../icon-provider/icon-provider'
import { TextAreaEventHandler } from '../../events/component-events';


@Component({
  tag: 'elsa-textarea-question',
  shadow: false,
})

export class ElsaTextAreaComponent {

  @Prop() question: TextAreaQuestion
  @State() iconProvider = new IconProvider();

  handler: TextAreaEventHandler;

  supportedSyntaxes: Array<string> = [SyntaxNames.JavaScript, SyntaxNames.Liquid];
  multiExpressionEditor: HTMLElsaMultiExpressionEditorElement;
  syntaxMultiChoiceCount: number = 0;

  @Event({
    eventName: 'updateQuestion',
    composed: true,
    cancelable: true,
    bubbles: true,
  }) updateQuestion: EventEmitter<IQuestionComponent>;

  async componentWillLoad() {
    if (this.question && !this.question.characterLimit) {
      this.question.characterLimit = 2000;
    }
    this.handler = new TextAreaEventHandler(this.question, this.updateQuestion);
  }

  renderQuestionField(fieldId, fieldName, fieldValue, onChangedFunction, isDisabled = false) {
    return <div>
      <div class="elsa-mb-1">
        <div class="elsa-flex">
          <div class="elsa-flex-1">
            <label htmlFor={fieldId} class="elsa-block elsa-text-sm elsa-font-medium elsa-text-gray-700">
              {fieldName}
            </label>
          </div>
        </div>
      </div>
      <input type="text" id={fieldId} name={fieldId} disabled={isDisabled} value={fieldValue} onChange={e => {
        onChangedFunction.bind(this)(e);
      }
      }
        class="disabled:elsa-opacity-50 disabled:elsa-cursor-not-allowed focus:elsa-ring-blue-500 focus:elsa-border-blue-500 elsa-block elsa-w-full elsa-min-w-0 elsa-rounded-md sm:elsa-text-sm elsa-border-gray-300" />
    </div>;
  }

  renderNumberQuestionField(fieldId, fieldName, fieldValue, onChangedFunction, isDisabled = false) {
    return <div>
      <div class="elsa-mb-1">
        <div class="elsa-flex">
          <div class="elsa-flex-1">
            <label htmlFor={fieldId} class="elsa-block elsa-text-sm elsa-font-medium elsa-text-gray-700">
              {fieldName}
            </label>
          </div>
        </div>
      </div>
      <input type="number" min="0" id={fieldId} name={fieldId} disabled={isDisabled} value={fieldValue} onChange={e => {
        onChangedFunction.bind(this)(e);
      }
      }
        class="disabled:elsa-opacity-50 disabled:elsa-cursor-not-allowed focus:elsa-ring-blue-500 focus:elsa-border-blue-500 elsa-block elsa-w-full elsa-min-w-0 elsa-rounded-md sm:elsa-text-sm elsa-border-gray-300" />
    </div>;
  }

  renderCheckboxField(fieldId, fieldName, isChecked, onChangedFunction) {
    return <div>
      <div class="elsa-mb-1 elsa-mt-2">
        <div class="elsa-flex">
          <div>
            <label htmlFor={fieldId} class="elsa-block elsa-text-sm elsa-font-medium elsa-text-gray-700 elsa-p-1">
              {fieldName}
            </label>
          </div>
          <div>
            <input id={fieldId} name={fieldId} type="checkbox" checked={isChecked} value={'true'} onChange={e =>
              onChangedFunction.bind(this)(e)}
              class="focus:elsa-ring-blue-500 elsa-h-8 elsa-w-8 elsa-text-blue-600 elsa-border-gray-300 elsa-rounded" />
          </div>
        </div>
      </div>
    </div>;
  }

  render() {
    const field = `question-${this.question.id}`;
    return (
      <div>
        {this.renderQuestionField(`${field}-questionid`, `Identifier`, this.question.id, this.handler.onIdentifierChanged, true)}
        {this.renderQuestionField(`${field}-title`, `Title`, this.question.title, this.handler.onTitleChanged)}
        {this.renderQuestionField(`${field}-questionText`, `Question`, this.question.questionText, this.handler.onQuestionChanged)}
        {this.renderQuestionField(`${field}-questionHint`, `Hint`, this.question.questionHint, this.handler.onHintChanged)}
        {this.renderQuestionField(`${field}-questionGuidance`, `Guidance`, this.question.questionGuidance, this.handler.onGuidanceChanged)}
        {this.renderCheckboxField(`${field}-displayCommentBox`, `Display Comments`, this.question.displayComments, this.handler.onDisplayCommentsBox)}
        {this.renderNumberQuestionField(`${field}-characterLimit`, `Character limit`, this.question.characterLimit, this.handler.onCharacterLimitChanged)}
      </div>
    );
  }
}