// JavaScript source code
import http from 'k6/http';
import { check, sleep } from 'k6';

export const options = {
    scenarios: {
        normal_load: {
            executor: 'constant-vus',
            vus: 20,
            duration: '30s',
        },
        peak_load: {
            executor: 'constant-vus',
            vus: 50,
            duration: '30s',
            startTime: '35s',
        },
        stress_load: {
            executor: 'constant-vus',
            vus: 100,
            duration: '30s',
            startTime: '70s',
        },
    },
    thresholds: {
        http_req_failed: ['rate<0.10'],
        http_req_duration: ['p(95)<2000'],
    },
};

const BASE_URL = 'https://localhost:7252';

export default function () {
    const uniqueClientId = `loadtest-client-${__VU}-${__ITER}-${Date.now()}`;

    const url =
        `${BASE_URL}/api/v1/guests/listings/search` +
        `?fromDate=2026-04-10` +
        `&toDate=2026-04-15` +
        `&noOfPeople=2` +
        `&country=Turkey` +
        `&city=Izmir` +
        `&pageNumber=1` +
        `&pageSize=10`;

    const params = {
        headers: {
            'X-Client-Id': uniqueClientId,
        },
    };

    const res = http.get(url, params);

    check(res, {
        'query status is 200': (r) => r.status === 200,
    });

    sleep(1);
}