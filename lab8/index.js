const mountebank = require('mountebank')
const settings = require('./settings');
const mountebankHelper = require('./mountebank-helper');

const mountbankServerInstance = mountebank.create({
    port: settings.port,
    pidfile: '../mb.pid',
    logfile: '../mb.log',
    protofile: '../protofile.json',
    ipWhitelist: ['*']
});

mountbankServerInstance.then(() => {
    mountebankHelper.postImposter(createImposter())
})

function createImposter()
{
    stubs = [
        {
            predicates: [ {
                equals: {
                    method: "GET",
                    "path": "/rate/usd"
                }
            }],

            responses: [
                {
                    is: {
                        statusCode: 200,
                        headers: {
                            "Content-Type": "application/json"
                        },
                        body: {rate: 76.08}
                    }
                }
            ]
        },

        {
            predicates: [ {
                equals: {
                    method: "GET",
                    "path": "/rate/euro"
                }
            }],

            responses: [
                {
                    is: {
                        statusCode: 200,
                        headers: {
                            "Content-Type": "application/json"
                        },
                        body: {rate: 91.02}
                    }
                }
            ]
        },

        {           
            predicates: [ {
                equals: {
                    method: "GET",
                    "path": "/rate/pound"
                }
            }],
            responses: [
                {
                    is: {
                        statusCode: 200,
                        headers: {
                            "Content-Type": "application/json"
                        },
                        body: {rate: 101.23}
                    }
                }
            ]
        }
    ];

    imposter = {
        port: settings.exchange_rate_service_port,
        protocol: 'http',
        stubs: stubs
    };

    return imposter
}