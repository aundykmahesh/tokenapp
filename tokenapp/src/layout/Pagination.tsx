import { observer } from 'mobx-react-lite';
import React, { SyntheticEvent } from 'react'
import { Menu, Pagination } from 'semantic-ui-react'
import { useStore } from '../stores/stores'

interface Props{
    tokenarranylength:number;
}

export default observer (function PaginationForToken({tokenarranylength} : Props){

const {tokenstore} = useStore();

function handleActivityChangeStatus(e: SyntheticEvent<HTMLElement>) {
  tokenstore.loadPaginatedTokens(parseInt(e.currentTarget.innerText))
}
let totalpages=(tokenarranylength/10);
if(tokenarranylength%10>0) totalpages +=1;
let index=0;
let menuitems=[];
for (index = 1; index <= totalpages; index++) {
  var local=index;
  menuitems.push(<Menu.Item as='a' key={index} name={index.toString()} 
    onClick={(e) => { handleActivityChangeStatus(e) }}>
      {index}
    </Menu.Item>)
}
return <Menu floated='right' pagination>{menuitems}</Menu> ;

})