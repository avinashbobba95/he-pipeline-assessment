export function AuthPlugin(elsaStudio) {

  const { eventBus } = elsaStudio;

  const getAccessToken = async () => {
    console.log("Get access token");
    const options = {
      grant_type: "client_credentials",
      client_id: "D4ZLiiwsBq9tMZXbCx2TwBUiNDcsXuIy",
      audience: "https://elsa-server-api",
      client_secret: "HsE3sWT3kvmsTiH1nUp1CImz9IRb2YxUN6wNopp1RWyKfrvMzeBkOWe9NkT_CG1L"
    };
    const params = {
      grant_type: 'client_credentials',
      client_id: options.client_id,
      client_secret: options.client_secret,
      audience:options.audience
    };

    const response = await fetch("https://" + "identity-staging-homesengland.eu.auth0.com" + "/oauth/token",
      {
        method: 'POST',
        body: JSON.stringify(params),
        headers: {
          "content-type": "application/x-www-form-urlencoded",
          "access-control-allow-origin": "*",
          "access-control-allow-methods": "POST, PUT, PATCH, GET, DELETE, OPTIONS",
          "Access-Control-Allow-Headers": "Origin, X-Requested-With, Content-Type, Accept, Authorization, access-control-allow-headers, access-control-allow-methods, access-control-allow-origin",
          
        }   
      })


    console.log(response);
  };

  // Handle the "http-client-created" event so we con configure the http client. 
  eventBus.on('http-client-created', async e => {
    console.log("authorization set up");
    const token = await getAccessToken();
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
