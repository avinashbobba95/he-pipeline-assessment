import 

export function AuthorizationMiddlewarePlugin(elsaStudio) {
    const eventBus = elsaStudio.eventBus;

    eventBus.on('http-client-created', e => {
      // Register Axios middleware.
      e.service.register({
        onRequest(request) {
          request.headers = { 'Authorization': 'Bearer 1234' }
          return request;
        }
      });
    });
  }
