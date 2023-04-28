import { SyntaxNames } from "../constants/constants";
import { HeActivityPropertyDescriptor } from "../models/custom-component-models";
import { ActivityDefinitionProperty, ActivityModel } from "../models/elsa-interfaces";

export type Map<T> = {
  [key: string]: T
};

export function ToLetter(num: number) {
  "use strict";
  var mod = num % 26,
    pow = num / 26 | 0,
    out = mod ? String.fromCharCode(64 + mod) : (--pow, 'Z');
  return pow ? ToLetter(pow) + out : out;
};

export function getOrCreateProperty(activity: ActivityModel, name: string, defaultExpression?: () => string, defaultSyntax?: () => string): ActivityDefinitionProperty {
  let property: ActivityDefinitionProperty = activity.properties.find(x => x.name == name);

  if (!property) {
    const expressions = {};
    let syntax = defaultSyntax ? defaultSyntax() : undefined;

    if (!syntax)
      syntax = SyntaxNames.Literal;

    expressions[syntax] = defaultExpression ? defaultExpression() : undefined;
    property = { name: name, expressions: expressions, syntax: null };
    activity.properties.push(property);
  }

  return property;
}

export function parseJson(json: string): any {
  if (!json)
    return null;

  try {
    return JSON.parse(json);
  } catch (e) {
    //console.warn(`Error parsing JSON: ${e}`);
    //The Above never parses quite correctly,
    e = e;
  }
  return undefined;
}

export function mapSyntaxToLanguage(syntax: string): any {
  switch (syntax) {
    case SyntaxNames.Json:
      return 'json';
    case SyntaxNames.JavaScript:
      return 'javascript';
    case SyntaxNames.Liquid:
      return 'liquid';
    case SyntaxNames.SQL:
      return 'sql';
    case SyntaxNames.Literal:
    default:
      return 'plaintext';
  }
}

export function filterPropertiesByType(questionProperties: Array<HeActivityPropertyDescriptor>, questionType: string) {
  console.log("Filtering properties", questionType);
  console.log("Properties to filter", questionProperties);
  const propertiesJson: string = JSON.stringify(questionProperties);
  const properties: Array<HeActivityPropertyDescriptor> = JSON.parse(propertiesJson);
  console.log("Random filter", properties.filter((x) => x.displayInDesigner == true))
  console.log("Alternative filter", properties.filter((x) => x.expectedOutputType === 'string'));
  console.log("HE Properties", properties);
  properties.forEach(p => {
    if (p.conditionalActivityTypes != null && p.conditionalActivityTypes.length > 0) {
      console.log("Has Conditional properties");
      if (!p.conditionalActivityTypes.includes(questionType)){
        p.displayInDesigner = false;
        console.log("includes the question type", p)
      }
    }
  });
  const filteredProperties: Array<HeActivityPropertyDescriptor> = properties.filter((property) => property.displayInDesigner === true);
  console.log("Filtered properties in method", filteredProperties);
  return filteredProperties;
}

export async function awaitElement(selector) {
  while (document.querySelector(selector) === null) {
    await new Promise(resolve => requestAnimationFrame(resolve))
  }
  return document.querySelector(selector);
}
