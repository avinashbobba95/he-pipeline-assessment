import { RegisterCustomPlugins } from './Plugins/CustomPlugins.js';
import { CustomPropertyUIHints } from './Constants/CustomPropertyUiHints.js';
import { CustomComponentTags } from './Constants/CustomComponentTags.js';
import { QuestionDriver } from './Drivers/QuestionPropertyDriver.js';
import { CustomSwitchDriver } from './Drivers/CustomSwitchPropertyDriver.js';
import { CustomTextDriver } from './Drivers/CustomTextPropertyDriver.js';
import { ConditionalTextListDriver } from './Drivers/ConditionalTextListPropertyDriver.js';
import { TextActivityDriver } from './Drivers/TextActivityPropertyDriver.js';
export function InitCustomElsa(elsaStudioRoot, customProperties) {

  elsaStudioRoot.addEventListener('initializing', e => {
    var elsaStudio = e.detail;
    RegisterPlugins(elsaStudio);
    RegisterDrivers(elsaStudio, customProperties);
   // AuthorizationMiddlewarePlugin(elsaStudio);
  });

  function RegisterDrivers(elsaStudio, customProperties) {
    elsaStudio.propertyDisplayManager.addDriver(CustomPropertyUIHints.QuestionScreenBuilder,
      () => new QuestionDriver(CustomComponentTags.QuestionScreen, customProperties));

    elsaStudio.propertyDisplayManager.addDriver(CustomPropertyUIHints.TextActivityBuilder,
      () => new TextActivityDriver(elsaStudio, CustomComponentTags.TextActivity));

    elsaStudio.propertyDisplayManager.addDriver(CustomPropertyUIHints.ConditionalTextListBuilder,
      () => new ConditionalTextListDriver(elsaStudio, CustomComponentTags.ConditionalTextList));

    elsaStudio.propertyDisplayManager.addDriver(CustomPropertyUIHints.CustomTextBuilder,
      () => new CustomTextDriver(elsaStudio, CustomComponentTags.CustomTextArea));

    elsaStudio.propertyDisplayManager.addDriver(CustomPropertyUIHints.CustomSwitchBuilder,
      () => new CustomSwitchDriver(elsaStudio, CustomComponentTags.CustomSwitch));
    
  }

  function RegisterPlugins(elsaStudio) {
    elsaStudio.pluginManager.registerPlugin(RegisterCustomPlugins);
  }

  async function AuthorizationMiddlewarePlugin(elsaStudio){ 
    const eventBus = elsaStudio.eventBus;
    const options = {
      domain: "https://identity-staging-homesengland.eu.auth0.com/",
      client_id: "D4ZLiiwsBq9tMZXbCx2TwBUiNDcsXuIy",
      audience: "https://elsa-server-api",    
    };
    console.log("I am here");

    //const auth0Client = await createAuth0Client(options);
   // console.log("Auth0Client" + auth0Client);
  //var token = await auth0Client.getTokenSilently()
    eventBus.on('http-client-created', e => {
      // Register Axios middleware.
      e.service.register({
        onRequest(request) {
          request.headers = {
            'Authorization': `Token 1234`
          }

          return request;
        }
      });
    });
  }

 }
