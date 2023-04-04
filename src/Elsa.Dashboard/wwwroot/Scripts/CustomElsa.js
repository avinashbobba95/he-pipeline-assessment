import { RegisterCustomPlugins } from './Plugins/CustomPlugins.js';
import { CustomPropertyUIHints } from './Constants/CustomPropertyUiHints.js';
import { CustomComponentTags } from './Constants/CustomComponentTags.js';
import { QuestionDriver } from './Drivers/QuestionPropertyDriver.js';
import { CustomSwitchDriver } from './Drivers/CustomSwitchPropertyDriver.js';
import { CustomTextDriver } from './Drivers/CustomTextPropertyDriver.js';
import { ConditionalTextListDriver } from './Drivers/ConditionalTextListPropertyDriver.js';
import { TextActivityDriver } from './Drivers/TextActivityPropertyDriver.js';
/*import { createAuth0Client } from '@auth0/auth0-spa-js';*/

export function InitCustomElsa(elsaStudioRoot, customProperties) {

  elsaStudioRoot.addEventListener('initializing', e => {
    var elsaStudio = e.detail;
    RegisterPlugins(elsaStudio);
    RegisterDrivers(elsaStudio, customProperties);
   /* Authenticate();*/
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

  //async function Authenticate() {
  //  const auth0 = new Auth0Client({
  //    domain: 'identity-staging-homesengland.eu.auth0.com',
  //    clientId: 'D4ZLiiwsBq9tMZXbCx2TwBUiNDcsXuIy',
  //    authorizationParams: {
  //      redirect_uri: 'http://localhost:7079'
  //    }
  //  });

  //  //if you do this, you'll need to check the session yourself
  //  try {
  //    var token = await auth0.getTokenSilently();
  //    console.log('token', token)
  //  } catch (error) {
  //    if (error.error !== 'login_required') {
  //      throw error;
  //    }
  //  }

  //} 

 }
